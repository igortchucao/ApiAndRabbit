using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Models;
using Teste.Repositorio.Repository;
using Teste.Aplicacao.Elasticserch;
using Microsoft.Extensions.Configuration;

namespace Teste.TestesUnitarios.Elastic;

[TestFixture]
public class ElasticTest
{
    private ElasticApplication _elastic;
    private Mock<IElasticsearchRepo> _elasticMock;
    private Mock<IConfiguration> _configMock;

    [SetUp]
    public void Setup()
    {
        _elasticMock = new Mock<IElasticsearchRepo>();
        _configMock = new Mock<IConfiguration>();
        _elastic = new ElasticApplication(_configMock.Object, _elasticMock.Object);
    }

    [Test]
    public void BuscarListaElastic_DeveRetornarListaQuandoNaoVazia()
    {
        // Arrange
        var listaBeneficiarios = new List<Beneficiario>() { new Beneficiario(), new Beneficiario() };
        _elasticMock.Setup(e => e.LerListaElastic()).Returns(listaBeneficiarios);

        // Act
        var resultado = _elastic.BuscarListaElastic() as OkObjectResult;

        // Assert
        Assert.IsNotNull(resultado);
        Assert.AreEqual(200, resultado.StatusCode);
        Assert.AreEqual(listaBeneficiarios, resultado.Value);
    }

    [Test]
    public void BuscarListaElastic_DeveRetornarNotFoundQuandoVazia()
    {
        // Arrange
        _elasticMock.Setup(e => e.LerListaElastic()).Returns(new List<Beneficiario>());

        // Act
        var resultado = _elastic.BuscarListaElastic() as NotFoundObjectResult;

        // Assert
        Assert.IsNotNull(resultado);
        Assert.AreEqual(404, resultado.StatusCode);
        Assert.AreEqual("Lista do elasticsearch está vazia", resultado.Value);
    }

    [Test]
    public void BuscarListaElastic_DeveRetornarBadRequestQuandoErro()
    {
        // Arrange
        _elasticMock.Setup(e => e.LerListaElastic()).Throws(new System.Exception());

        // Act
        var resultado = _elastic.BuscarListaElastic() as BadRequestObjectResult;

        // Assert
        Assert.IsNotNull(resultado);
        Assert.AreEqual(400, resultado.StatusCode);
        Assert.AreEqual("Erro ao buscar Lista no elasticserch", resultado.Value);
    }
}
