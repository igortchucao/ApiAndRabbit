using System.Text.Json.Serialization;

namespace Teste.Models.Models
{
    public class Autentificador
    {
        [JsonPropertyName("data")]
        public Data Data { get; set; }
    }

    public class Data
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("expireIn")]
        public DateTime ExpirenIn { get; set; }
    }
}
