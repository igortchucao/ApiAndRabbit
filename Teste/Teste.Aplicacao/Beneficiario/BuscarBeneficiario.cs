using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Models;
using System.Net.Http.Headers;
using System.Text.Json;
using Teste.Repositorio.Repository;
using Teste.Repositorio.Service;

namespace Teste.Aplicacao.Beneficiarios;

public class BuscarBeneficiario : BaseRota, IBuscarBeneficio
{
    public const string BUSCAR_BENEFICIARIO = "api/v1/inss/consulta-beneficios";
    public ICacheRedis _cacheRedis { get; set; }
    public IElasticsearchRepo _elastic { get; set; }
    private HttpClient _httpClient { get; set; }

    public BuscarBeneficiario(IConfiguration configuration, ICacheRedis cacheRedis, IElasticsearchRepo elastic) : base(configuration) 
    {
        _cacheRedis = cacheRedis;
        _elastic = elastic;
        _httpClient = new HttpClient();
    }

    public BuscarBeneficiario(IConfiguration configuration, ICacheRedis cacheRedis, IElasticsearchRepo elastic, HttpClient httpClient) : base(configuration)
    {
        _cacheRedis = cacheRedis;
        _elastic = elastic;
        _httpClient = httpClient;
    }

    public async Task<IActionResult> BuscarBeneficiarioAsync(string cpf)
    {
        var beneficiarioReturn = new Beneficiario();

        var cachedValue = _cacheRedis.LerCache(cpf);

        if (cachedValue != string.Empty)
        {
            beneficiarioReturn = JsonSerializer.Deserialize<Beneficiario>(cachedValue);
        }
        else
        {
            using (_httpClient)
            {
                try
                {
                    await AutentificarAsync();

                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token.Data.Token);

                    HttpResponseMessage response = await _httpClient.GetAsync(ApiUrl + BUSCAR_BENEFICIARIO + $"?cpf={cpf}");

                    if (response.IsSuccessStatusCode)
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        beneficiarioReturn = JsonSerializer.Deserialize<Beneficiario>(content);

                        _cacheRedis.GuardarCache(cpf, beneficiarioReturn);
                        _elastic.BeneficiarioAddElastic(beneficiarioReturn);
                    }
                    else
                    {
                        return new NotFoundObjectResult("Beneficiario não encontrado");
                    }
                }
                catch (Exception ex)
                {
                    return new BadRequestObjectResult("Erro ao buscar beneficiário: " + ex.Message);
                }
            }
        }

        return new OkObjectResult(beneficiarioReturn);
    }

    public void SetHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
}
