using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using WireMock.Server;

namespace GestionCasos.Api.Tests;

public class BasePruebasIntegracion : IDisposable
{
    public HttpClient Cliente { get; private set; }
    public WireMockServer MockServer { get; set; }

    private readonly IServiceScope _serviceScope;
    private readonly ApiApp WebApp;
    private bool disposed = false;
    public readonly IMongoDatabase _database;

    public BasePruebasIntegracion()
    {
        WebApp = new ApiApp();

        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Testing");

        var serviceCollection = WebApp.GetServiceCollection();
        _serviceScope = serviceCollection.CreateScope();
        MockServer = _serviceScope.ServiceProvider.GetRequiredService<WireMockServer>();
        Cliente = WebApp.CreateClient();
        Cliente.DefaultRequestHeaders.Add("X-Platform-Key", "c80f8c92-5b8d-4aea-bef4-8bfcf018bdab");
        Cliente.DefaultRequestHeaders.Add("X-Project-Key", "valor_proyecto_simulado");

        var mongoClient = WebApp.Services.GetService<IMongoClient>()!;
        _database = mongoClient.GetDatabase(WebApp.DatabaseName);
    }


    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                // Liberar recursos gestionados
                Cliente.Dispose();
                MockServer.Stop();
                WebApp.Dispose();
                _serviceScope.Dispose();
            }

            // Liberar recursos no gestionados (si los hubiera)
            disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
