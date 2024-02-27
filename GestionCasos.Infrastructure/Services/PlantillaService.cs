using GestionCasos.Domain.Entities;
using GestionCasos.Domain.Ports;
using GestionCasos.Infrastructure.Adapters;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace gestion_casos.Infrastructure.Services;

[Repository]
public class PlantillaService : IPlantillaService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _baseUrl;
    private readonly ILogMessageService _resourceManager;
    private readonly ConfiguracionNotificacion _configuracionApiNotificacion;

    public PlantillaService(IHttpClientFactory httpClientFactory, IOptions<ConfiguracionNotificacion> configuracionCorreoNotificacionOptions,
        ILogMessageService resourceManager, ConfiguracionNotificacion configuracionApiNotificacion)
    {
        _httpClientFactory = httpClientFactory;
        var configuracionCorreoNotificacion = configuracionCorreoNotificacionOptions.Value;
        _baseUrl = $"{configuracionCorreoNotificacion.URL}{configuracionCorreoNotificacion.TemplateUrl}";
        _resourceManager = resourceManager;
        _configuracionApiNotificacion = configuracionApiNotificacion;
    }

    public async Task<TemplateResponse> ObtenerPlantillaAsync(string nombrePlantilla, string nombrePlataforma)
    {
        var httpClient = _httpClientFactory.CreateClient();
        var url = $"{_baseUrl}?name={nombrePlantilla}&platformName={nombrePlataforma}";
        httpClient.DefaultRequestHeaders.Add(_configuracionApiNotificacion.HeaderName, _configuracionApiNotificacion.Key);

        var response = await httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{_resourceManager.ErrorObtencionPlantilla} {response.StatusCode}");
        }


        var content = await response.Content.ReadAsStringAsync();
        var templateApiResponse = JsonConvert.DeserializeObject<PlantillaApiRespuesta>(content)!;

        return templateApiResponse.Response;
    }
}
