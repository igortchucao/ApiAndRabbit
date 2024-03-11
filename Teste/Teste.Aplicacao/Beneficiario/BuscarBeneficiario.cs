using Microsoft.Extensions.Configuration;
using Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Teste.Aplicacao.Beneficiarios
{
    public class BuscarBeneficiario : BaseRota, IBuscarBeneficio
    {
        public const string BUSCAR_BENEFICIARIO = "api/v1/inss/consulta-beneficios";

        public BuscarBeneficiario(IConfiguration configuration) : base(configuration) {}

        public async Task<Beneficiario> BuscarBeneficiarioAsync(string cpf)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    await AutentificarAsync();

                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

                    HttpResponseMessage response = await httpClient.GetAsync(ApiUrl + BUSCAR_BENEFICIARIO + $"?cpf={cpf}");

                    if (response.IsSuccessStatusCode)
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        Beneficiario beneficiario = JsonSerializer.Deserialize<Beneficiario>(content);

                        return beneficiario;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Erro ao buscar beneficiário: " + ex.Message);
                }
            }
        }
    }
}
