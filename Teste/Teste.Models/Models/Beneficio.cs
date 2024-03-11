using System.Text.Json.Serialization;

namespace Models
{
    public class Beneficio
    {
        [JsonPropertyName("numero_beneficio")]
        public string NumeroBeneficio { get; set; }
        [JsonPropertyName("codigo_tipo_beneficio")]
        public string TipoBeneficio { get; set; }
    }
}
