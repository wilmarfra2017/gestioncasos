using GestionCasos.Domain.Utils.GeneradorId;
using GestionCasos.Infrastructure.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System.Collections.Immutable;
using System.Globalization;

namespace GestionCasos.Infrastructure.Extensions;

public static class Dependencias
{
    public static IServiceCollection CargarDependencias(this IServiceCollection services, IConfiguration config)
    {
        services.AddMongoClient(config)
                .AddJiraConfiguration(config)
                .AddHttpConfiguration(config)
                .AddStorageConfiguration(config)
                .AddStaticConfigurations(config)
                .AddTransient<IGeneradorNumeroAleatorio, GeneradorNumeroAleatorio>();

        return services;
    }

    private static IServiceCollection AddMongoClient(this IServiceCollection services, IConfiguration config)
    {
        var mongoCon = Environment.GetEnvironmentVariable("GESTIONCASOS_CONEXION") ?? config.GetConnectionString("GestorCasosDb");
        services.AddSingleton<IMongoClient, MongoClient>(serviceProvider => new MongoClient(mongoCon));

        var nombreDb = Environment.GetEnvironmentVariable("GESTIONCASOS_BD") ?? config.GetSection("GestorBaseDatos").Value;
        var mongoConfig = new ClienteMongoConfig
        {
            NombreBaseDatos = nombreDb
        };
        services.AddSingleton(mongoConfig);

        return services;
    }

    private static IServiceCollection AddJiraConfiguration(this IServiceCollection services, IConfiguration config)
    {
        var urlJira = Environment.GetEnvironmentVariable("JIRA_URL") ?? config.GetSection("Jira:Url").Value;
        var usuarioJira = Environment.GetEnvironmentVariable("JIRA_USERNAME") ?? config.GetSection("Jira:Usuario").Value;
        var passwordJira = Environment.GetEnvironmentVariable("JIRA_PASSWORD") ?? config.GetSection("Jira:Password").Value;
        var jiraEndpoint = Environment.GetEnvironmentVariable("JIRA_ENDPOINT") ?? config.GetSection("Jira:JiraEnpoint").Value;

        var jiraConfig = new JiraConfig
        {
            Url = new Uri(urlJira!),
            Username = usuarioJira!,
            Password = passwordJira!,
            Base_Endpoint = jiraEndpoint!,
        };

        services.AddSingleton(jiraConfig);

        return services;
    }

    private static IServiceCollection AddHttpConfiguration(this IServiceCollection services, IConfiguration config)
    {
        var proyectoHeaderName = Environment.GetEnvironmentVariable("PROYECTO_HEADER_NAME") ?? config.GetSection("HttpHeaders:ProyectoNombre").Value;
        var plataformaHeaderName = Environment.GetEnvironmentVariable("PLATAFORMA_HEADER_NAME") ?? config.GetSection("HttpHeaders:PlataformaNombre").Value;
        var logCampos = config.GetSection("LoggingCampos").Get<List<string>>()?.ToImmutableArray() ?? [];
        var httpConfig = new HttpConfig
        (
            proyectoHeaderName!,
            plataformaHeaderName!,
            logCampos
        );
        services.AddSingleton(httpConfig);
        return services;
    }
    private static IServiceCollection AddStorageConfiguration(this IServiceCollection services, IConfiguration config)
    {
        var storage = new StorageConfig { CandenaConexion = Environment.GetEnvironmentVariable("STORAGE_URL") ?? config.GetSection("Storage:CadenaConexion").Value };
        services.AddSingleton(storage);
        return services;
    }
    private static IServiceCollection AddStaticConfigurations(this IServiceCollection services, IConfiguration config)
    {
        ApiConfig.PrefixUrl = Environment.GetEnvironmentVariable("API_RUTA_PREFIJO") ?? config.GetSection("ApiRutaPrefijo").Value!;
        JiraCamposConfig.SolicitudPadreId = config.GetSection("JiraCampos:SolicitudPadreId").Value!;
        ArchivoConfig.PesoMax = Convert.ToInt32(Environment.GetEnvironmentVariable("PESO_MAX") ?? config.GetSection("Archivo:PesoMax_MB").Value, CultureInfo.InvariantCulture);
        return services;
    }
}


