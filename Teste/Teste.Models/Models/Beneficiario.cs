using System.Text.Json.Serialization;

namespace Models
{
    public class Beneficiario
    {
        [JsonPropertyName("data")]
        public Data Data { get; set; }
        
        public Beneficiario() { }
    }

    public class Data
    {
        [JsonPropertyName("cpf")]
        public string Cpf { get; set; }
        [JsonPropertyName("beneficios")]
        public List<Beneficio> Beneficios { get; set; } = new List<Beneficio>();

    }
}
