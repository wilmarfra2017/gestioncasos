using GestionCasos.Domain.Ports;
using GestionCasos.Infrastructure.Config;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Net.Mime;

namespace GestionCasos.Api.Middleware;

public class ApiKeyValidationHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IPlataformaRepository _plataformaRepository;
    private readonly HttpConfig _httpConfig;
    private readonly ILogMessageService _resourceManager;
    private readonly WebHookConfig _config;

    public ApiKeyValidationHandlerMiddleware(
        RequestDelegate next,
        IPlataformaRepository plataformaRepository,
        HttpConfig httpConfig,
        ILogMessageService resourceManager,
        IOptions<WebHookConfig> config)
    {
        _next = next;
        _plataformaRepository = plataformaRepository;
        _httpConfig = httpConfig;
        _resourceManager = resourceManager;
        _config = config.Value;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (EsSolicitudWebHookExcluida(context))
        {
            await _next.Invoke(context);
            return;
        }

        if (!context.Request.Headers.TryGetValue(_httpConfig.PlataformaHeaderName, out var extractedApiKey) ||
            !context.Request.Headers.TryGetValue(_httpConfig.ProyectoHeaderName, out var extractedProjectKey))
        {
            await WriteUnauthorizedResponseAsync(context, _resourceManager.ErrorValidacionApiKeyNotFound);
            return;
        }

        if (await IsUnathorizedAsync(extractedApiKey, extractedProjectKey))
        {
            string errorMessage = _resourceManager.ErrorValidacionApiKeyInvalid;
            await WriteUnauthorizedResponseAsync(context, errorMessage);
            return;
        }
        context.Items[_httpConfig.ProyectoHeaderName] = extractedProjectKey;
        await _next.Invoke(context);
    }

    private async Task<bool> IsUnathorizedAsync(string? apiKey, string? projectKey)
    {
        if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(projectKey) || !Guid.TryParse(apiKey, CultureInfo.InvariantCulture, out var guid))
        {
            return true;
        }
        var plataformaKey = await _plataformaRepository.BuscarPorKeyAsync(apiKey);
        return plataformaKey is null;
    }

    private static async Task WriteUnauthorizedResponseAsync(HttpContext context, string message)
    {
        var result = System.Text.Json.JsonSerializer.Serialize(new
        {
            StatusCode = StatusCodes.Status401Unauthorized,
            Content = message
        });
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        context.Response.ContentType = MediaTypeNames.Application.Json;
        await context.Response.WriteAsync(result);
    }

    private bool EsSolicitudWebHookExcluida(HttpContext context) =>
    context.Request.Method == HttpMethods.Post &&
    context.Request.Host.Host.Equals(_config.Host, StringComparison.OrdinalIgnoreCase) &&
    context.Request.Path.ToString().Equals(_config.Path, StringComparison.OrdinalIgnoreCase);
}

