using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Teste.Repositorio.Repository;

namespace Teste.Aplicacao.Elasticserch;

public class ElasticApplication : BaseRota, IElasticApplication
{
    public IElasticsearchRepo _elastic { get; set; }
    public ElasticApplication(IConfiguration configuration, IElasticsearchRepo elasticsearchRepo) : base(configuration)
    {
        _elastic = elasticsearchRepo;
    }

    public IActionResult BuscarListaElastic()
    {
        try
        {
            var beneficiarios = _elastic.LerListaElastic();

            if(beneficiarios.Count > 0)
                return new OkObjectResult(beneficiarios);

            return new NotFoundObjectResult("Lista do elasticsearch está vazia");
        }
        catch
        {
            return new BadRequestObjectResult("Erro ao buscar Lista no elasticserch");
        }
    }
}
