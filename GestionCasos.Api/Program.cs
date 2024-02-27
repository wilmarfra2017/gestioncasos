using FluentValidation;
using gestion_casos.Application.Features.Notificaciones.Commands.EnviarSmsNotificacion;
using GestionCasos.Api.Filters;
using GestionCasos.Api.Middleware;
using GestionCasos.Api.Validators;
using GestionCasos.Application.Features.Adjuntos.Commands.AdjuntarDocumento;
using GestionCasos.Application.Features.Catalogos.Queries.ObtenerCatalogos;
using GestionCasos.Application.Features.EstadosSolicitudes.Commands;
using GestionCasos.Application.Features.Notificaciones.Commands.EnviarCorreoNotificacion;
using GestionCasos.Application.Features.Notificaciones.Commands.EnviarSmsNotificacion;
using GestionCasos.Application.Features.Solicitudes.Commands.ActualizarSolicitud;
using GestionCasos.Application.Features.Solicitudes.Commands.CrearSolicitud;
using GestionCasos.Application.Features.Solicitudes.Queries.ObtenerSolicitudes;
using GestionCasos.Infrastructure.Config;
using GestionCasos.Infrastructure.Extensions;
using Prometheus;
using Serilog;
using Serilog.Events;
using System.Reflection;
using System.Resources;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services.AddValidatorsFromAssemblyContaining<Program>(ServiceLifetime.Singleton);

builder.Services.AddHttpClient();
builder.Services.AddControllers(opts =>
{
    opts.Filters.Add(typeof(ValidateAttribute));
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builderx => builderx.AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            );
});

builder.Services.CargarDependencias(config);
builder.Services.CargarServicios(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "Humano Seguros  Casos SAN", Version = "version 1.0.0" });
    options.DocumentFilter<BasePathFilter>(config["Swagger:RutaBaseFilter"]);
    options.CustomSchemaIds(type => type.FullName);
});
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.Load("gestion_casos.Application")));
builder.Services.AddTransient<IValidator<CrearSolicitudesCommand>, CrearSolicitudCommandValidator>();
builder.Services.AddTransient<IValidator<ObtenerCatalogosQuery>, ObtenerCatalogosQueryValidator>();
builder.Services.AddTransient<IValidator<CargarAdjuntoCommand>, CargarAdjuntoCommandValidator>();
builder.Services.AddTransient<IValidator<ObtenerSolicitudesQuery>, ObtenerSolicitudesQueryValidator>();
builder.Services.AddTransient<IValidator<ActualizarSolicitudCommand>, ActualizarSolicitudCommandValidator>();
builder.Services.AddTransient<IValidator<EnviarCorreoNotificacionCommand>, EnviarCorreoNotificacionCommandValidator>();
builder.Services.AddTransient<IValidator<EnviarSmsNotificacionCommand>, EnviarSmsNotificacionCommandValidator>();

builder.Services.AddSingleton<ResourceManager>(new ResourceManager("gestion_casos.Infrastructure.Resources.ErrorMessages", typeof(Program).Assembly));
builder.Services.AddSingleton<ResourceManager>(new ResourceManager("gestion_casos.Api.Resources.ErrorMessages", typeof(Program).Assembly));
builder.Services.AddSingleton<ResourceManager>(new ResourceManager("gestion_casos.Infrastructure.Resources.SettingMessages", typeof(Program).Assembly));
builder.Services.AddTransient<IValidator<ActualizarEstadosSolicitudCommand>, EstadosSolicitudesValidator>();
builder.Services.Configure<NotificacionConfig>(config.GetSection("Notificacion"));
builder.Services.Configure<WebHookConfig>(config.GetSection("Webhook"));

string? logUrlDatabase = null;
string? logCollectionName = null;

logUrlDatabase = Environment.GetEnvironmentVariable("LOG_DATABASE_URL") ?? config.GetSection("Serilog:DatabaseUrl").Value;

ArgumentException.ThrowIfNullOrEmpty(logUrlDatabase);
logCollectionName = Environment.GetEnvironmentVariable("LOG_COLLECTION_NAME") ?? config.GetSection("Serilog:CollectionName").Value;
ArgumentException.ThrowIfNullOrEmpty(logCollectionName);

builder.Host.UseSerilog((_, loggerConfiguration) =>
    loggerConfiguration
        .WriteTo.MongoDBBson(databaseUrl: $"{logUrlDatabase}",
                             collectionName: logCollectionName!,
                             restrictedToMinimumLevel: LogEventLevel.Information
                             ));


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => options.SwaggerEndpoint(config["Swagger:RutaJson"], "Humano Seguros Casos SAN"));
}

app.Use(async (context, next) =>
{
    context.Response.Headers.Append("Content-Security-Policy", "img-src 'self' data:;default-src 'self' 'unsafe-inline'; style-src 'self' 'unsafe-inline';");
    context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Append("X-Frame-Options", "DENY");
    context.Response.Headers.Append("X-Permitted-Cross-Domain-Policies", "master-only");
    context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");
    context.Response.Headers.Append("Cache-Control", "no-cache,no-store,must-revalidate");
    context.Response.Headers.Append("Pragma", "no-cache");
    context.Response.Headers.Remove("X-Powered-By");
    context.Response.Headers.Remove("Server");
    await next();
});

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseHttpMetrics();

app.UseMiddleware<AppExceptionHandlerMiddleware>();
//app.UseMiddleware<ApiKeyValidationHandlerMiddleware>();

app.UseRouting().UseEndpoints(endpoint => { endpoint.MapMetrics(); });

app.MapControllers();

app.Run();
