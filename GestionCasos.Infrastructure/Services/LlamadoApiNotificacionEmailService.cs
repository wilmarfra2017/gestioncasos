using GestionCasos.Domain.Dtos;
using GestionCasos.Domain.Entities;
using GestionCasos.Domain.Ports;
using GestionCasos.Infrastructure.Adapters;
using GestionCasos.Infrastructure.Config;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;
using System.Text;
using System.Text.Json;

namespace GestionCasos.Infrastructure.Services;
[Repository]
public class LlamadoApiNotificacionEmailService(IHttpClientFactory httpClientFactory,
                                          IOptions<NotificacionConfig> _config,
                                          ILogMessageService resourceManager) : INotificacionApiEmailService
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly NotificacionConfig _notificationConfig = _config!.Value;
    private readonly ILogMessageService _resourceManager = resourceManager;

   
    public async Task<bool> EnviarCorreoNotificacionAsync(NotificacionEmailRequestDto notificacionEmailRequestDto)
    {
        AsyncRetryPolicy<HttpResponseMessage> retryPolicy = Policy
        .Handle<HttpRequestException>()
        .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
        .WaitAndRetryAsync(_notificationConfig.MaximoReintentos, retryAttempt => TimeSpan.FromSeconds(Math.Pow(_notificationConfig.FrecuenciaReintentos, retryAttempt)));

        try
        {
            var httpClient = _httpClientFactory.CreateClient();
            var url = new Uri($"{_notificationConfig.Host}{_notificationConfig.RutaPrefijo}{_notificationConfig.EnpointEmail}");
            var jsonRequest = JsonSerializer.Serialize(notificacionEmailRequestDto);
            var content = new StringContent(jsonRequest, Encoding.UTF8, _resourceManager.ContentType);
            var response = await retryPolicy.ExecuteAsync(async () =>
            {
                var httpResponse = await httpClient.PostAsync(url, content);
                httpResponse.EnsureSuccessStatusCode();
                return httpResponse;
            });

            var responseBody = await response.Content.ReadAsStringAsync();
            return responseBody is not null;
        }
        catch (HttpRequestException)
        {
            return false;
        }
    }
}

