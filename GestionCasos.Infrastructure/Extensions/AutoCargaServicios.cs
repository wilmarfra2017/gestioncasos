using gestion_casos.Infrastructure.Services;
using GestionCasos.Domain.Entities;
using GestionCasos.Domain.Ports;
using GestionCasos.Domain.Services;
using GestionCasos.Infrastructure.Adapters;
using GestionCasos.Infrastructure.Ports;
using GestionCasos.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace GestionCasos.Infrastructure.Extensions;

public static class AutoCargaServicios
{
    public static IServiceCollection CargarServicios(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient(typeof(IRepository<>), typeof(GenericRepository<>));
        services.AddSingleton(typeof(ILogMessageService), typeof(LogMessageService));
        services.AddSingleton(typeof(INotificacionEmailService), typeof(NotificacionEmailService));
        services.AddSingleton(typeof(IEstructuraCorreoService), typeof(EstructuraCorreoNotificacionService));
        services.AddSingleton(typeof(INotificacionSmsService), typeof(NotificacionSmsService));
        services.AddSingleton(typeof(IEstructuraSmsService), typeof(EstructuraSmsNotificacionService));


        services.Configure<ConfiguracionNotificacion>(configuration.GetSection("NotificacionApi"));
        services.AddSingleton(provider => provider.GetRequiredService<IOptions<ConfiguracionNotificacion>>().Value);
        services.Configure<DominioHumano>(configuration.GetSection("DominioHumano"));
        services.AddSingleton(provider => provider.GetRequiredService<IOptions<DominioHumano>>().Value);


        var _services = AppDomain.CurrentDomain.GetAssemblies()
              .Where(assembly =>
              {
                  return (assembly.FullName is null) || assembly.FullName.Contains("Domain", StringComparison.InvariantCulture);
              })
              .SelectMany(s => s.GetTypes())
              .Where(p => p.CustomAttributes.Any(x => x.AttributeType == typeof(DomainServiceAttribute)));

        // Ditto, but repositories
        var _repositories = AppDomain.CurrentDomain.GetAssemblies()
            .Where(assembly =>
            {
                return (assembly.FullName is null) || assembly.FullName.Contains("Infrastructure", StringComparison.InvariantCulture);
            })
            .SelectMany(s => s.GetTypes())
            .Where(p => p.CustomAttributes.Any(x => x.AttributeType == typeof(RepositoryAttribute)));

        // svc
        foreach (var service in _services)
        {
            services.AddTransient(service);
        }

        // repos
        foreach (var repo in _repositories)
        {
            Type iface = repo.GetInterfaces().Single();
            services.AddTransient(iface, repo);
        }

        return services;
    }
}
