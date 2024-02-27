using GestionCasos.Api.Tests.DataMock;
using GestionCasos.Api.Tests.Notificaciones.DataBuilder;
using GestionCasos.Domain.Entities;
using Newtonsoft.Json;
using System.Text;

namespace GestionCasos.Api.Tests.Notificaciones;

public class NotificacionesSmsControllerTests : IClassFixture<BasePruebasIntegracion>
{
    private readonly BasePruebasIntegracion _basePruebas;

    public NotificacionesSmsControllerTests(BasePruebasIntegracion basePruebas)
    {
        _basePruebas = basePruebas;
        basePruebas.Cliente.DefaultRequestHeaders.Add("ApiKey", "bcc0991b-175c-495a-b99f-c01438e87d9f");
    }

    [Fact]
    public async Task EnviarSmsApiNotification_Exito()
    {
        EndpointsMock.ConfigurarWireMockSmsNotificacion(_basePruebas);
        EndpointsMock.ConfigurarWireMockSmsPlantilla(_basePruebas);

        var notificacionDtoBuilder = new NotificacionSmsRequestDtoBuilder();
        var notificacionDto = notificacionDtoBuilder
            .WithNumero("8091234567")
            .WithComentario("Te falta foto titular en ticket JRASERVER-2001 creado en Jira")
            .WithEstatus(new Estatus("0001", "Pendiente por documentacion"))
            .WithNombreIntermediario("Wilmar Martinez")
            .WithSolicitudkey("JRASERVER-2001")
            .Build();

        var json = JsonConvert.SerializeObject(notificacionDto, Formatting.Indented);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _basePruebas.Cliente.PostAsync("/api/v1/gestion-casos/notificacionesSms", content);

        response.EnsureSuccessStatusCode();

        Assert.True(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task EnviarNotificacionSms_FalloPorDatosFaltantes()
    {
        var notificacionDtoBuilder = new NotificacionSmsRequestDtoBuilder();
        var notificacionDto = notificacionDtoBuilder
            .WithNumero("")
            .WithComentario("Este comentario no se enviará porque falta el numero de celular")
            .WithEstatus(new Estatus("0001", "Pendiente por documentacion"))
            .WithNombreIntermediario("Wilmar Martinez")
            .WithSolicitudkey("JRASERVER-2001")
            .Build();

        var json = JsonConvert.SerializeObject(notificacionDto, Formatting.Indented);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _basePruebas.Cliente.PostAsync("/api/v1/gestion-casos/notificacionesSms", content);

        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task EnviarNotificacionSms_FalloPorNumeroInvalido()
    {
        var notificacionDtoBuilder = new NotificacionSmsRequestDtoBuilder();
        var notificacionDto = notificacionDtoBuilder
            .WithNumero("1234567890")
            .WithComentario("Este comentario no se enviará debido al numero de celular inválido")
            .WithEstatus(new Estatus("0001", "Pendiente por documentacion"))
            .WithNombreIntermediario("Wilmar Martinez")
            .WithSolicitudkey("JRASERVER-2000")
            .Build();

        var json = JsonConvert.SerializeObject(notificacionDto, Formatting.Indented);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _basePruebas.Cliente.PostAsync("/api/v1/gestion-casos/notificacionesSms", content);

        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
