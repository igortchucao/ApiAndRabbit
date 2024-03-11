using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Text;
using Teste.Models.Models;

namespace Teste.Aplicacao
{
    public class BaseRota : IBaseRota
    {
        public string ApiUrl { get; set; }
        public readonly IConfiguration _configuration;
        public string Token { get; set; }

        public BaseRota(IConfiguration configuration)
        {
            _configuration = configuration;
            ApiUrl = _configuration["apiUrl"];
        }

        public async Task AutentificarAsync()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var user = new
                    {
                        username = _configuration["UserData:User"],
                        password = _configuration["UserData:Key"]
                    };

                    string json = JsonSerializer.Serialize(user);

                    HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(ApiUrl + "api/v1/token", content);

                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    Autentificador auth = JsonSerializer.Deserialize<Autentificador>(jsonResponse);
                    Token = auth.Data.Token;

                }
            }catch
            {
                throw new Exception("Erro ao Autentificar");
            }
        }
    }
}