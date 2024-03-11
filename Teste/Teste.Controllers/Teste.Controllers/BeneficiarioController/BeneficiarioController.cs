using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Teste.Aplicacao.Beneficiarios;

namespace Teste.Controllers.BeneficiarioController;

[ApiController]
[Route("api/[controller]")]
public class BeneficiarioController : ControllerBase
{
    public IBuscarBeneficio _buscarBeneficio { get; set; }
    public BeneficiarioController(IBuscarBeneficio buscarBeneficio)
    {
        _buscarBeneficio = buscarBeneficio;
    }
    /// <summary>
    /// Obtém todos os itens.
    /// </summary>
    /// <returns>Uma lista de itens.</returns>
    
    [HttpGet]
    [Route("obter/{cpf}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAsync(string cpf)
    {
        try 
        { 
            var beneficiario = await _buscarBeneficio.BuscarBeneficiarioAsync(cpf);

            if(beneficiario == null)
                return new NotFoundResult();
            
            return Ok(beneficiario);

        }catch
        {
            return new BadRequestResult();
        }

    }
}
