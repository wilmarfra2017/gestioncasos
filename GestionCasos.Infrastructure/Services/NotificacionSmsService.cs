using GestionCasos.Domain.Entities;
using GestionCasos.Domain.Exceptions;
using GestionCasos.Domain.Ports;
using GestionCasos.Infrastructure.Adapters;
using Newtonsoft.Json;
using System.Text;

namespace GestionCasos.Infrastructure.Services;

[Repository]
public class NotificacionSmsService(ConfiguracionNotificacion _configuracionNotificacion,
    IHttpClientFactory _httpClientFactory, ILogMessageService _resourceManager) : INotificacionSmsService
{

    public async Task<bool> EnviarSmsNotificacionAsync(EstructuraSmsNotificacion notificacionSmsRequestDto)
    {
        var httpClient = _httpClientFactory.CreateClient();
        var url = $"{_configuracionNotificacion.URL}{_configuracionNotificacion.SmsSendUrl}";

        var objetoParaEnviar = new
        {
            toDestination = notificacionSmsRequestDto.NumeroDestinatario,
            notificationType = _resourceManager.Sms,
            providerName = notificacionSmsRequestDto.NombreProveedor,
            template = new
            {
                name = notificacionSmsRequestDto.Plantilla.Nombre,
                platformName = notificacionSmsRequestDto.Plantilla.NombrePlataforma,
                metadata = notificacionSmsRequestDto.Plantilla.Metadatos.Select(m => new
                {
                    key = m.Key,
                    m.description,
                    m.isRequired,
                    value = m.Value
                }).ToArray(),
                language = notificacionSmsRequestDto.Plantilla.Idioma
            }
        };

        var json = JsonConvert.SerializeObject(objetoParaEnviar);
        using var content = new StringContent(json, Encoding.UTF8, "application/json");

        httpClient.DefaultRequestHeaders.Add(_configuracionNotificacion.HeaderName, _configuracionNotificacion.Key);

        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, url) { Content = content };
            using var response = await httpClient.SendAsync(request);
            return response.IsSuccessStatusCode;
        }
        catch (HttpRequestException ex)
        {
            throw new PersistenciaException($"{_resourceManager.ErrorNotificacionSms} {ex.Message}");
        }
    }
}
