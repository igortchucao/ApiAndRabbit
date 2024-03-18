using NUnit.Framework;
using Moq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Models;
using Teste.Repositorio.Service;
using Teste.Aplicacao.Beneficiarios;
using Teste.Repositorio.Repository;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Moq.Protected;
using Teste.Models.Models;
using Nest;

namespace Teste.TestesUnitarios.Beneficiarios;

[TestFixture]
public class BeneficiarioTeste
{
    private BuscarBeneficiario _beneficiario;
    private Mock<ICacheRedis> _cacheRedisMock;
    private Mock<IElasticsearchRepo> _elasticMock;
    private Mock<IConfiguration> _confiMock;
    private Mock<HttpClient> _httpClientMock;

    [SetUp]
    public void Setup()
    {
        _cacheRedisMock = new Mock<ICacheRedis>();
        _elasticMock = new Mock<IElasticsearchRepo>();
        _confiMock = new Mock<IConfiguration>();
        _httpClientMock = new Mock<HttpClient>();

        _beneficiario = new BuscarBeneficiario(_confiMock.Object, _cacheRedisMock.Object, _elasticMock.Object, _httpClientMock.Object);
        _beneficiario.ApiUrl = "http://teste-dev-api-dev-140616584.us-east-1.elb.amazonaws.com/";
    }

    [Test]
    public async Task BuscarBeneficiarioAsync_DeveRetornarBeneficiarioQuandoCacheExistir()
    {
        // Arrange
        var cpf = "415.022.590-79";
        
        var beneficiarioEsperado = new Beneficiario();
        beneficiarioEsperado.Data.Cpf = "41502259079";
        beneficiarioEsperado.Data.Beneficios.Add(new Beneficio() { NumeroBeneficio = "3240776714", TipoBeneficio = "96" });

        var cachedValue = JsonSerializer.Serialize(beneficiarioEsperado);
        _cacheRedisMock.Setup(x => x.LerCache(cpf)).Returns(cachedValue);

        // Act
        var resultado = await _beneficiario.BuscarBeneficiarioAsync(cpf) as OkObjectResult; ;

        // Assert 
        Assert.IsNotNull(resultado);
        Assert.That(resultado.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));

        var beneficiario = resultado.Value as Beneficiario;
        Assert.That(beneficiario.Data.Cpf, Is.EqualTo(beneficiarioEsperado.Data.Cpf));
        Assert.That(beneficiario.Data.Beneficios.Count, Is.EqualTo(beneficiarioEsperado.Data.Beneficios.Count));

    }

    [Test]
    public async Task BuscarBeneficiarioAsync_DeveRetornarBeneficiarioQuandoEncontrado()
    {
        // Arrange
        string cpf = "415.022.590-79";
        var beneficiarioEsperado = new Beneficiario();
        beneficiarioEsperado.Data.Cpf = "41502259079";
        beneficiarioEsperado.Data.Beneficios.Add(new Beneficio() { NumeroBeneficio = "3240776714", TipoBeneficio = "96" });

        var cachedValue = JsonSerializer.Serialize(beneficiarioEsperado);
        _cacheRedisMock.Setup(x => x.LerCache(cpf)).Returns(string.Empty);

        _beneficiario.Token.Data.ExpirenIn = DateTime.UtcNow.AddDays(10);

        var handlerMock = new Mock<HttpMessageHandler>();
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(cachedValue),
        };

        handlerMock
           .Protected()
           .Setup<Task<HttpResponseMessage>>(
              "SendAsync",
              ItExpr.IsAny<HttpRequestMessage>(),
              ItExpr.IsAny<CancellationToken>())
           .ReturnsAsync(response);
        var httpClient = new HttpClient(handlerMock.Object);

        _beneficiario.SetHttpClient(httpClient);

        // Act
        var resultado = await _beneficiario.BuscarBeneficiarioAsync(cpf) as OkObjectResult;

        // Assert
        Assert.IsNotNull(resultado);
        Assert.That(resultado.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));

        var beneficiario = resultado.Value as Beneficiario;
        Assert.That(beneficiario.Data.Cpf, Is.EqualTo(beneficiarioEsperado.Data.Cpf));
        Assert.That(beneficiario.Data.Beneficios.Count, Is.EqualTo(beneficiarioEsperado.Data.Beneficios.Count));
    }

    [Test]
    public async Task BuscarBeneficiarioAsync_DeveRetornarQuandoNaoEncontrado()
    {
        // Arrange
        string cpf = "415.022.590-78";

        _cacheRedisMock.Setup(x => x.LerCache(cpf)).Returns(string.Empty);

        _beneficiario.Token.Data.ExpirenIn = DateTime.UtcNow.AddDays(10);

        var handlerMock = new Mock<HttpMessageHandler>();
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.NotFound,
        };

        handlerMock
           .Protected()
           .Setup<Task<HttpResponseMessage>>(
              "SendAsync",
              ItExpr.IsAny<HttpRequestMessage>(),
              ItExpr.IsAny<CancellationToken>())
           .ReturnsAsync(response);
        var httpClient = new HttpClient(handlerMock.Object);

        _beneficiario.SetHttpClient(httpClient);

        // Act
        var resultado = await _beneficiario.BuscarBeneficiarioAsync(cpf) as NotFoundObjectResult;

        // Assert
        Assert.IsNotNull(resultado);
        Assert.That(resultado.StatusCode, Is.EqualTo((int)HttpStatusCode.NotFound));

    }

    [Test]
    public void BuscarBeneficiarioAsync_DeveLancarExcecaoQuandoErroNoProcessamento()
    {
        // Arrange
        string cpf = "415.022.590-79";
        _cacheRedisMock.Setup(x => x.LerCache(cpf)).Throws(new Exception("Erro simulado ao ler o cache"));

        // Act & Assert
        Assert.ThrowsAsync<Exception>(() => _beneficiario.BuscarBeneficiarioAsync(cpf));
    }
}
