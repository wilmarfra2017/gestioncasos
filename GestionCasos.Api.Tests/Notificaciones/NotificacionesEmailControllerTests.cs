using GestionCasos.Api.Tests.DataMock;
using GestionCasos.Api.Tests.Notificaciones.DataBuilder;
using GestionCasos.Domain.Entities;
using Newtonsoft.Json;
using System.Text;


namespace GestionCasos.Api.Tests.Notificaciones;
public class NotificacionesEmailControllerTests : IClassFixture<BasePruebasIntegracion>
{
    private readonly BasePruebasIntegracion _basePruebas;

    public NotificacionesEmailControllerTests(BasePruebasIntegracion basePruebas)
    {
        _basePruebas = basePruebas;
        basePruebas.Cliente.DefaultRequestHeaders.Add("ApiKey", "bcc0991b-175c-495a-b99f-c01438e87d9f");
    }


    [Fact]
    public async Task EnviarCorreoApiNotification_Exito()
    {
        EndpointsMock.ConfigurarWireMockEmailNotificacion(_basePruebas);
        EndpointsMock.ConfigurarWireMockCorreoPlantilla(_basePruebas);

        var notificacionDtoBuilder = new NotificacionEmailRequestDtoBuilder();
        var notificacionDto = notificacionDtoBuilder
            .WithEmail("wilframar@gmail.com")
            .WithComentario("Te falta la cedula en el ticket JRASERVER-2000 creado en Jira")
            .WithEstatus(new Estatus("0001", "Pendiente por documentacion"))
            .WithNombreIntermediario("Wilmar Martinez")
            .WithSolicitudkey("JRASERVER-2000")
            .Build();

        var json = JsonConvert.SerializeObject(notificacionDto, Formatting.Indented);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _basePruebas.Cliente.PostAsync("/api/v1/gestion-casos/notificacionesEmail", content);

        response.EnsureSuccessStatusCode();

        Assert.True(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task EnviarNotificacionCorreo_FalloPorDatosFaltantes()
    {
        var notificacionDtoBuilder = new NotificacionEmailRequestDtoBuilder();
        var notificacionDto = notificacionDtoBuilder
            .WithEmail("")
            .WithComentario("Este comentario no se enviará porque falta el correo electrónico")
            .WithEstatus(new Estatus("0001", "Pendiente por documentacion"))
            .WithNombreIntermediario("Wilmar Martinez")
            .WithSolicitudkey("JRASERVER-2000")
            .Build();

        var json = JsonConvert.SerializeObject(notificacionDto, Formatting.Indented);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _basePruebas.Cliente.PostAsync("/api/v1/gestion-casos/notificacionesEmail", content);

        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task EnviarNotificacionCorreo_FalloPorCorreoInvalido()
    {
        var notificacionDtoBuilder = new NotificacionEmailRequestDtoBuilder();
        var notificacionDto = notificacionDtoBuilder
            .WithEmail("correoInvalido")
            .WithComentario("Este comentario no se enviará debido a un correo electrónico inválido")
            .WithEstatus(new Estatus("0001", "Pendiente por documentacion"))
            .WithNombreIntermediario("Wilmar Martinez")
            .WithSolicitudkey("JRASERVER-2000")
            .Build();

        var json = JsonConvert.SerializeObject(notificacionDto, Formatting.Indented);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _basePruebas.Cliente.PostAsync("/api/v1/gestion-casos/notificacionesEmail", content);

        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }



}
