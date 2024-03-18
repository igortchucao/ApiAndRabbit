using Microsoft.Extensions.Configuration;
using Models;
using Nest;

namespace Teste.Repositorio.Repository;

public class ElasticsearchRepo : IElasticsearchRepo
{
    public string Uri { get; set; }
    public readonly IConfiguration _configuration;

    public ElasticsearchRepo(IConfiguration configuration)
    {
        _configuration = configuration;
        Uri = _configuration["UriElastic"]; ;
    }

    public void BeneficiarioAddElastic(Beneficiario beneficiario)
    {
        try
        {
            var settings = new ConnectionSettings(new Uri(Uri))
                .DefaultIndex("cpf");

            var client = new ElasticClient(settings);
            var response = client.IndexDocument(beneficiario);

            if (!response.IsValid)
            {
                throw new Exception(response.ServerError?.Error?.Reason);
            }
        }
        catch(Exception e) 
        {
            throw new Exception("Erro ao acessar o Elasticsearch");
        }
    }


    public IList<Beneficiario> LerListaElastic()
    {
        var settings = new ConnectionSettings(new Uri(Uri))
            .DefaultIndex("cpf"); 

        var client = new ElasticClient(settings);

        int tamanho = 10;
        bool next = true;

        List<Beneficiario> beneficiarios = new List<Beneficiario>();

        while (next)
        {
            var beneficiariosElastic = client.Search<Beneficiario>(s => s
                .From(0)
                .Size(tamanho)
                .Query(q => q.MatchAll())); 

            if (beneficiariosElastic.IsValid)
            {
                var hits = beneficiariosElastic.Hits;
                if (hits.Any())
                {
                    foreach (var hit in hits)
                    {
                        beneficiarios.Add(hit.Source);
                    }
                }
                else
                {
                    next = false;
                }
            }
            else
            {
                throw new Exception("Erro ao recuperar os documentos");
            }
        }

        return beneficiarios;
    }
}
