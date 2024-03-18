using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Teste.Aplicacao.Beneficiarios;
using Teste.Aplicacao.Elasticserch;
using Teste.Aplicacao.Rabbit;

namespace Teste.Controllers.BeneficiarioController;

[ApiController]
[Route("api/[controller]")]
public class BeneficiarioController : ControllerBase
{
    public IBuscarBeneficio _buscarBeneficio { get; set; }
    public IElasticApplication _elastic { get; set; }
    public IRabbitMessage _rabbit { get; set; }
    public BeneficiarioController(IBuscarBeneficio buscarBeneficio, IRabbitMessage rabbit, IElasticApplication elastic)
    {
        _buscarBeneficio = buscarBeneficio;
        _rabbit = rabbit;
        _elastic = elastic;
    }
    /// <summary>
    /// Obtém Beneficiário a partir do CPF
    /// </summary>
    /// <returns>Um beneficiário com CPF e uma lista de benefícios.</returns>

    [HttpGet]
    [Route("obter/{cpf}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAsync(string cpf)
    {
        return await _buscarBeneficio.BuscarBeneficiarioAsync(cpf);
    }

    /// <summary>
    /// Preenche a lista do rabbit a partir dos CPFs enviados no e-mail
    /// </summary>
    /// <returns>Ok ou badRequst</returns>

    [HttpPost]
    [Route("/rabbit/preencher")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PostPreencherAsync()
    {
        return await _rabbit.PreencherListaRabbit();
    }

    /// <summary>
    /// Obtém Beneficiário a partir da lista do Rabbit
    /// </summary>
    /// <returns>Um beneficiário com CPF e uma lista de benefícios.</returns>

    [HttpGet]
    [Route("rabbit/ler")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetLerAsync()
    {
        return await _rabbit.LerListaRabbit();
    }

    /// <summary>
    /// Obtém Beneficiário a partir da lista do Rabbit
    /// </summary>
    /// <returns>Um beneficiário com CPF e uma lista de benefícios.</returns>

    [HttpGet]
    [Route("elasticsearch/lerLista")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetElasticsearchAsync()
    { 
        return _elastic.BuscarListaElastic();
    }
}
