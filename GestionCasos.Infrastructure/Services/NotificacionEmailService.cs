using GestionCasos.Domain.Dtos;
using GestionCasos.Domain.Entities;
using GestionCasos.Domain.Exceptions;
using GestionCasos.Domain.Ports;
using GestionCasos.Infrastructure.Adapters;
using Newtonsoft.Json;
using System.Text;

namespace gestion_casos.Infrastructure.Services;

[Repository]
public class NotificacionEmailService(ConfiguracionNotificacion configuracionApiNotificacion,
                                IHttpClientFactory _httpClientFactory,
                                ILogMessageService _resourceManager) : INotificacionEmailService
{
    private readonly ConfiguracionNotificacion _configuracionApiNotificacion = configuracionApiNotificacion;

    public async Task<bool> EnviarCorreoNotificacionAsync(EstructuraCorreoNotificacion estructuraNotificacion)
    {
        var httpClient = _httpClientFactory.CreateClient();
        var url = $"{_configuracionApiNotificacion.URL}{_configuracionApiNotificacion.EmailSendUrl}";

        var objetoParaEnviar = new
        {
            toEmail = estructuraNotificacion.CorreoDestinatario,
            ccEmails = Array.Empty<string>(),
            bccEmails = Array.Empty<string>(),
            providerName = estructuraNotificacion.NombreProveedor,
            template = new
            {
                name = estructuraNotificacion.Plantilla.Nombre,
                platformName = estructuraNotificacion.Plantilla.NombrePlataforma,
                metadata = estructuraNotificacion.Plantilla.Metadatos.Select(m => new
                {
                    key = m.Key,
                    m.description,
                    m.isRequired,
                    value = m.Value
                }).ToArray(),
                language = estructuraNotificacion.Plantilla.Idioma
            }
        };

        var json = JsonConvert.SerializeObject(objetoParaEnviar);
        using var content = new StringContent(json, Encoding.UTF8, "application/json");

        httpClient.DefaultRequestHeaders.Add(_configuracionApiNotificacion.HeaderName, _configuracionApiNotificacion.Key);

        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, url) { Content = content };
            using var response = await httpClient.SendAsync(request);
            return response.IsSuccessStatusCode;
        }
        catch (HttpRequestException ex)
        {
            throw new PersistenciaException($"{_resourceManager.ErrorNotificacionCorreo} {ex.Message}");
        }
    }
}
