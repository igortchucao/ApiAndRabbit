using Microsoft.OpenApi.Models;
using Teste.Aplicacao;
using Teste.Repositorio.Repository;
using Teste.Repositorio.Service;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddAuthorization();
        services.AddControllers();

        services.AddSingleton<ICacheRedis, CacheRedis>();
        services.AddSingleton<IElasticsearchRepo, ElasticsearchRepo>();

        var classeBaseType = typeof(BaseRota);
        var types = classeBaseType.Assembly.ExportedTypes//.ToList()[0];
            .Where(p => p.BaseType is not null && p.BaseType.Name == classeBaseType.Name);

        foreach (var type in types)
        {
            var implementedInterfaces = type.GetInterfaces();
            var implementedInterface = implementedInterfaces.FirstOrDefault(i => i != typeof(IBaseRota));

            if (implementedInterface != null)
            {
                services.AddScoped(implementedInterface, type);
            }
        }

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Teste.Api", Version = "v1" });
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Teste.Api");
        });
    }
}