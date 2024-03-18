using Microsoft.AspNetCore.Mvc;

namespace Teste.Aplicacao.Rabbit
{
    public interface IRabbitMessage
    {
        Task<IActionResult> LerListaRabbit();
        Task<IActionResult> PreencherListaRabbit();
    }
}