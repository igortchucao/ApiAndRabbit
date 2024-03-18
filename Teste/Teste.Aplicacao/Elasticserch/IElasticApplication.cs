using Microsoft.AspNetCore.Mvc;
using Models;

namespace Teste.Aplicacao.Elasticserch
{
    public interface IElasticApplication
    {
        IActionResult BuscarListaElastic();
    }
}