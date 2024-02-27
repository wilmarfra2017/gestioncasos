using GestionCasos.Api.Dtos;
using GestionCasos.Api.Tests.DataMock;
using GestionCasos.Api.Tests.Solicitudes.DataBuilder;
using GestionCasos.Api.Tests.Utils;
using GestionCasos.Application.Dtos;
using GestionCasos.Domain.Entities;
using MongoDB.Driver;
using System.Net.Http.Headers;
using System.Text;

namespace GestionCasos.Api.Tests.Solicitudes;

public class SolicitudControllerTests : IClassFixture<BasePruebasIntegracion>
{
    private readonly BasePruebasIntegracion basePruebas;

    public SolicitudControllerTests(BasePruebasIntegracion basePruebasIntegracion)
    {
        basePruebas = basePruebasIntegracion;
    }

    [Fact]
    public async Task CrearSolicitud_Success()
    {
        var solicitudRequestDtoBuilder = new SolicitudRequestDtoBuilder()
            .ConResumen("Resumen de la solicitud")
            .ConTipoSolicitud(new Dtos.TipoSolicitudDto(Id: "1", Nombre: "Tipo de Solicitud"))
            .ConUsuarioId("usuario123")
            .ConCampos(new Dictionary<string, object> { { "customfield_11309", "prueba nuevo campo 14" } })
            .ConDescripcion("Descripción opcional de la solicitud");
        var expectValueResumen = "Resumen de la solicitud";
        var solicitudDtoBuilder = new SolicitudesRequestDtoBuilder()
            .ConPrefijoSolicitud("MOV-SL")
            .ConSolicitudes(new List<SolicitudRequestDto> { solicitudRequestDtoBuilder.Build() });

        var solicitudDto = solicitudDtoBuilder.Build();

        EndpointsMock.ConfigurarWireMockGuardar(basePruebas);
        EndpointsMock.ConfigurarWireMockConsultar(basePruebas);

        var response = await basePruebas.Cliente.PostAsJsonAsync("/api/v1/gestion-casos/solicitudes", solicitudDto);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(content);
        var solicitudesCreadasElement = doc.RootElement.GetProperty("data").GetProperty("solicitudesCreadas");
        var solicitudesCreadas = JsonSerializer.Deserialize<List<SolicitudDto>>(solicitudesCreadasElement.GetRawText(),
           JsonOptions.DefaultJsonSerializerOptions);

        Assert.NotNull(solicitudesCreadas);
        Assert.NotEmpty(solicitudesCreadas);
        solicitudesCreadas.ForEach(solicitudCreadaDto =>
        {
            Assert.NotNull(solicitudCreadaDto.SolicitudId);
            Assert.NotNull(solicitudCreadaDto.SolicitudKey);
        });

        var collection = basePruebas._database.GetCollection<Solicitud>("Solicitud");
        var solicitudCreada = await collection.Find(s => s.SolicitudId == solicitudesCreadas[0].SolicitudId).FirstOrDefaultAsync();

        Assert.NotNull(solicitudCreada);
        Assert.Equal(expectValueResumen, solicitudCreada.Resumen);
    }

    [Fact]
    public async Task ObtenerSolicitudes_DebeRetornarDataCuandoSeIngreseInformacionEnElQueryString()
    {
        var url = "/api/v1/gestion-casos/solicitudes?pagina=1&tamanoPagina=10&usuarioId=user123&solicitudKey=&estatusId=&tipoSolicitudId=";
        var response = await basePruebas.Cliente.GetAsync(url);
        var totalRegistrosEsperados = 1;

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var solicitudesResponse = JsonDocument.Parse(content).RootElement.GetProperty("data").ToString();
        var solicitudesPaginated = JsonSerializer.Deserialize<ResponsePaginadoDto<ObtenerSolicitudesDto>>(solicitudesResponse, JsonOptions.DefaultJsonSerializerOptions);

        Assert.Equal(totalRegistrosEsperados, solicitudesPaginated!.TotalRegistros);
        Assert.NotNull(solicitudesPaginated);
        Assert.IsType<ResponsePaginadoDto<ObtenerSolicitudesDto>>(solicitudesPaginated);
        Assert.True(solicitudesPaginated.Registros.Any());
    }

    [Fact]
    public async Task ObtenerSolicitudes_NoDebeRetornaDataCuandoIngreseInformacionEnElQueryStringYNoTengaCoincidenciaConLoGuardado()
    {
        var url = "/api/v1/gestion-casos/solicitudes?pagina=1&tamanoPagina=10&usuarioId=123&solicitudKey=&estatusId=&tipoSolicitudId=";
        var response = await basePruebas.Cliente.GetAsync(url);
        var totalRegistrosEsperados = 0;
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var solicitudesResponse = JsonDocument.Parse(content).RootElement.GetProperty("data").ToString();
        var solicitudesPaginated = JsonSerializer.Deserialize<ResponsePaginadoDto<ObtenerSolicitudesDto>>(solicitudesResponse, JsonOptions.DefaultJsonSerializerOptions);

        Assert.Equal(totalRegistrosEsperados, solicitudesPaginated!.TotalRegistros);
        Assert.IsType<ResponsePaginadoDto<ObtenerSolicitudesDto>>(solicitudesPaginated);
        Assert.False(solicitudesPaginated.Registros.Any());
    }

    [Fact]
    public async Task ObtenerSolicitudes_CuandoNoIngreseInformacionEnElQueryString_RetornaBadRequest()
    {
        var url = "/api/v1/gestion-casos/solicitudes?pagina=1&tamanoPagina=10&solicitudIdExterna=&solicitudKey=&estatusId=&tipoSolicitudId=";
        var response = await basePruebas.Cliente.GetAsync(url);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CargarAdjuntoSolicitud_Success()
    {
        EndpointsMock.ConfigurarWireMockActualizar(basePruebas);
        var fileName = "documento_prueba.pdf";
        var fileContent = new ByteArrayContent(Encoding.UTF8.GetBytes("Contenido del documento de prueba"));
        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/pdf");

        var formData = new MultipartFormDataContent();
        formData.Add(fileContent, "File", fileName);

        var solicitudId = "917813";
        var customTestField = "customfield";
        var response = await basePruebas.Cliente.PostAsync($"/api/v1/gestion-casos/solicitudes/{solicitudId}/{customTestField}/adjuntos", formData);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(content);
        var adjuntoElement = doc.RootElement.GetProperty("data");
        var adjuntoDto = JsonSerializer.Deserialize<AdjuntoDto>(adjuntoElement.GetRawText(),
           JsonOptions.DefaultJsonSerializerOptions);

        Assert.NotNull(adjuntoDto);
        Assert.Equal(fileName, adjuntoDto.NombreAdjunto);
        Assert.NotNull(adjuntoDto.UrlAdjunto);
    }

    [Fact]
    public async Task CargarAdjuntoSolicitud_DebeResponderBadRequest_CuandoNoExistaLaSolicitud()
    {
        var fileName = "documento_prueba.pdf";
        var fileContent = new ByteArrayContent(Encoding.UTF8.GetBytes("Contenido del documento de prueba"));
        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/pdf");
        var formData = new MultipartFormDataContent();
        formData.Add(fileContent, "File", fileName);
        var solicitudId = "111";
        var customTestField = "customfield";
        var response = await basePruebas.Cliente.PostAsync($"/api/v1/gestion-casos/solicitudes/{solicitudId}/{customTestField}/adjuntos", formData);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task ActualizarSolicitud_DebeModificarLaSolicitud_CuandoSeaValida()
    {
        EndpointsMock.ConfigurarWireMockActualizar(basePruebas);
        EndpointsMock.ConfigurarWireMockConsultar(basePruebas);
        var usuarioId = "user123";
        var solicitudActualDto = await ObtenerSolicitudAsync(usuarioId);
        var solicitudId = "917813";
        var campos = new Dictionary<string, object> { { "customfield_11309", "prueba campo editado" } };
        var solicitudDto = new ActualizacionRequestDto(
            campos
        );

        var response = await basePruebas.Cliente.PatchAsJsonAsync($"/api/v1/gestion-casos/solicitudes/{solicitudId}", solicitudDto);
        response.EnsureSuccessStatusCode();
        var solicitudModificadaDto = await ObtenerSolicitudAsync(usuarioId);

        Assert.NotNull(solicitudActualDto);
        Assert.NotNull(solicitudModificadaDto);
        Assert.NotEqual(solicitudActualDto, solicitudModificadaDto);
    }

    [Fact]
    public async Task ActualizarSolicitud_DebeResponderBadRequest_CuandoNoExistaLaSolicitud()
    {
        EndpointsMock.ConfigurarWireMockActualizar(basePruebas);
        EndpointsMock.ConfigurarWireMockConsultar(basePruebas);
        var solicitudId = "123";
        var campos = new Dictionary<string, object> { { "customfield_11309", "prueba campo editado" } };
        var solicitudDto = new ActualizacionRequestDto(
            campos
        );

        var response = await basePruebas.Cliente.PatchAsJsonAsync($"/api/v1/gestion-casos/solicitudes/{solicitudId}", solicitudDto);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    private async Task<ObtenerSolicitudesDto> ObtenerSolicitudAsync(string usuarioId)
    {
        var url = $"/api/v1/gestion-casos/solicitudes?pagina=1&tamanoPagina=10&usuarioId={usuarioId}&solicitudKey=&estatusId=&tipoSolicitudId=";
        var response = await basePruebas.Cliente.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var solicitudesResponse = JsonDocument.Parse(content).RootElement.GetProperty("data").ToString();
        var solicitudesPaginated = JsonSerializer.Deserialize<ResponsePaginadoDto<ObtenerSolicitudesDto>>(solicitudesResponse, JsonOptions.DefaultJsonSerializerOptions);
        return solicitudesPaginated?.Registros.FirstOrDefault()!;
    }
}