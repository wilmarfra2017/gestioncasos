using GestionCasos.Domain.Dtos;
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
public class LlamadoApiNotificacionSmsService(IHttpClientFactory httpClientFactory,
                                        IOptions<NotificacionConfig> _config,
                                        ILogMessageService resourceManager) : INotificacionApiSmsService
{

    private readonly NotificacionConfig _notificationConfig = _config!.Value;

    public async Task<bool> EnviarSmsNotificacionAsync(NotificacionSmsRequestDto notificacionSmsRequestDto)
    {
        AsyncRetryPolicy<HttpResponseMessage> retryPolicy = Policy
      .Handle<HttpRequestException>()
      .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
      .WaitAndRetryAsync(_notificationConfig.MaximoReintentos, retryAttempt => TimeSpan.FromSeconds(Math.Pow(_notificationConfig.FrecuenciaReintentos, retryAttempt)));

        try
        {
            var httpClient = httpClientFactory.CreateClient();
            var url = new Uri($"{_notificationConfig.Host}{_notificationConfig.RutaPrefijo}{_notificationConfig.EnpointSms}");
            var jsonRequest = JsonSerializer.Serialize(notificacionSmsRequestDto);
            var content = new StringContent(jsonRequest, Encoding.UTF8, resourceManager.ContentType);
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

