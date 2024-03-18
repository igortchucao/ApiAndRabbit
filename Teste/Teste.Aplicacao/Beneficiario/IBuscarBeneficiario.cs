using Microsoft.AspNetCore.Mvc;
using Models;

namespace Teste.Aplicacao.Beneficiarios
{
    public interface IBuscarBeneficio
    {
        Task<IActionResult> BuscarBeneficiarioAsync(string cpf);
    }
}