using gestion_casos.Infrastructure.Config;
using GestionCasos.Api.Tests.DataBuilder;
using GestionCasos.Api.Tests.DataMock;
using GestionCasos.Infrastructure.Adapters;
using GestionCasos.Infrastructure.Config;
using GestionCasos.Infrastructure.Ports;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mongo2Go;
using MongoDB.Bson;
using MongoDB.Driver;
using WireMock.Server;

namespace GestionCasos.Api.Tests;

internal class ApiApp : WebApplicationFactory<Program>, IDisposable
{
    private MongoDbRunner? mongoDbRunner;
    private WireMockServer? wiremockServer;
    private bool disposed = false;

    public string DatabaseName { get; } = $"TestDatabase_{Guid.NewGuid()}";

    public IServiceProvider GetServiceCollection()
    {
        return Services;
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        var jsonConfig = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
         .AddJsonFile("appsettings.test.json", false).Build();
        var azuriteKey = "Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;";
        var azuriteConnectionString = jsonConfig.GetSection("AZURITE_CONNECTION").Value;
        var azuriteConnection = string.Format(azuriteConnectionString!,azuriteKey);

        mongoDbRunner = MongoDbRunner.Start();
        var mongoClient = new MongoClient(mongoDbRunner.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(DatabaseName);
        
        wiremockServer = WireMockServer.Start();

        var jiraConfig = new JiraConfig();
        var notificacionConfig = new NotificacionConfig();
        var apiNotificacionConfig = new ApiNotificacionConfig();

        builder.ConfigureAppConfiguration(configBuilder =>
        {
            configBuilder.AddInMemoryCollection(new KeyValuePair<string, string?>[]
           {
                new("Jira:Url", wiremockServer.Urls[0]),
                new("Notificacion:Host", wiremockServer.Urls[0]),
                new("NotificacionApi:Url", wiremockServer.Urls[0])
           });
            configBuilder.Build().Bind("Jira", jiraConfig);
            configBuilder.Build().Bind("Notificacion", notificacionConfig);
        });

        builder.ConfigureServices(services =>
        {
            services.AddSingleton(wiremockServer);
            services.AddScoped<IBlobStorageRepository, BlobStorageRepository>();
            services.AddSingleton<IMongoClient, MongoClient>(serviceProvider => new MongoClient(mongoDbRunner.ConnectionString));
            var mongoConfig = new ClienteMongoConfig()
            {
                NombreBaseDatos = DatabaseName
            };
            services.AddSingleton(mongoConfig);

            InsertarDocumentoAutenticacion(mongoDatabase);
            InsertarDocumentoSolicitudes(mongoDatabase);
            InsertarDocumentoCatalogos(mongoDatabase);

            var jiraConfigService = services.SingleOrDefault(d => d.ServiceType == typeof(JiraConfig));
            if (jiraConfigService != null)
            {
                services.Remove(jiraConfigService);
            }
            var NotificacionConfigService = services.SingleOrDefault(d => d.ServiceType == typeof(NotificacionConfig));
            if (NotificacionConfigService != null)
            {
                services.Remove(NotificacionConfigService);
            }
            var apiNotificacionConfigService = services.SingleOrDefault(d => d.ServiceType == typeof(ApiNotificacionConfig));
            if (apiNotificacionConfigService != null)
            {
                services.Remove(apiNotificacionConfigService);
            }

            jiraConfig.Url = new Uri(wiremockServer.Urls[0]);
            notificacionConfig.Host = new Uri(wiremockServer.Urls[0]);
            services.AddSingleton(jiraConfig);

            apiNotificacionConfig.URL = new Uri(wiremockServer.Urls[0]);
            services.AddSingleton(apiNotificacionConfig);

            services.AddSingleton<IBlobStorageRepository>(serviceProvider =>
            {
                var config = new StorageConfig
                {
                    CandenaConexion = azuriteConnection
                };
                return new BlobStorageRepository(config);
            });
        });
        return base.CreateHost(builder);
    }
    private static void InsertarDocumentoAutenticacion(IMongoDatabase mongoDatabase)
    {
        var collection = mongoDatabase.GetCollection<BsonDocument>("Plataforma");
        var document = new BsonDocument
           {
               { "Nombre", "Oficina test" },
               { "Descripcion", "prueba" },
               { "Key", "c80f8c92-5b8d-4aea-bef4-8bfcf018bdab" },
               { "Activo", true }
           };
        collection.InsertOne(document);
        VerificarInsercionDocumento(collection, "c80f8c92-5b8d-4aea-bef4-8bfcf018bdab");
    }
    private static void InsertarDocumentoSolicitudes(IMongoDatabase mongoDatabase)
    {
        var collection = mongoDatabase.GetCollection<BsonDocument>("Solicitud");
        var document = SolicitudesCreadasMock.Obtener();
        collection.InsertOne(document);
    }

    private static void InsertarDocumentoCatalogos(IMongoDatabase mongoDatabase)
    {
        var collection = mongoDatabase.GetCollection<BsonDocument>("Catalogo");
        var document = CatalogosCreadosMock.Obtener();
        collection.InsertOne(document);
        var nombreCatalogo = document["Nombre"].AsString;
        VerificarInsercionCatalogo(mongoDatabase, nombreCatalogo);
    }

    private static void VerificarInsercionDocumento(IMongoCollection<BsonDocument> collection, string key)
    {
        var insertedDocument = collection.Find(new BsonDocument("Key", key)).FirstOrDefault();
        if (insertedDocument == null)
        {
            throw new InvalidOperationException($"No se pudo insertar el documento con Key: {key} en la colección.");
        }
    }

    private static void VerificarInsercionCatalogo(IMongoDatabase mongoDatabase, string nombreCatalogo)
    {
        var collection = mongoDatabase.GetCollection<BsonDocument>("Catalogo");
        var insertedDocument = collection.Find(new BsonDocument("Nombre", nombreCatalogo)).FirstOrDefault();
        if (insertedDocument == null)
        {
            throw new InvalidOperationException($"No se pudo insertar el documento del catálogo con Nombre: {nombreCatalogo} en la colección.");
        }
    }


    protected new void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                wiremockServer?.Stop();
                wiremockServer?.Dispose();
                mongoDbRunner?.Dispose();
            }
            disposed = true;
        }
    }


    public new void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
