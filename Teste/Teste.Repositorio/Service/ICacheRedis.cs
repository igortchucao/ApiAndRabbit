namespace Teste.Repositorio.Service;

public interface ICacheRedis
{
    public string? LerCache(string key);

    public void GuardarCache(string key, object value);
}
