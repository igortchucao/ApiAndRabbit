using Models;

namespace Teste.Repositorio.Repository
{
    public interface IElasticsearchRepo
    {
        void BeneficiarioAddElastic(Beneficiario beneficiario);
        IList<Beneficiario> LerListaElastic();
    }
}