using Models;

namespace Teste.Aplicacao.Beneficiarios
{
    public interface IBuscarBeneficio
    {
        Task<Beneficiario> BuscarBeneficiarioAsync(string cpf);
    }
}