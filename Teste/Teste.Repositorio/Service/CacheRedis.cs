using StackExchange.Redis;
using System.Text.Json;

namespace Teste.Repositorio.Service
{
    public class CacheRedis: ICacheRedis
    {
        public IDatabase db { get; set; }

        public CacheRedis()
        {
            try
            {
                ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost");

                db = redis.GetDatabase();
            }
            catch { }
        }

        public string? LerCache(string key)
        {
            if(db != null)
            {
                return db.StringGet(key);
            }

            return string.Empty;
        }

        public void GuardarCache(string key, object value)
        {
            if (db != null)
            {
                string message = JsonSerializer.Serialize(value);

                db.StringSet(key, message, TimeSpan.FromMinutes(1));
            }
        }
    }
}
