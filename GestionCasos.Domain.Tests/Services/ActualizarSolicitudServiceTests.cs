using GestionCasos.Api.Tests.DataBuilder;
using GestionCasos.Domain.Dtos;
using GestionCasos.Domain.Entities;
using GestionCasos.Domain.Ports;
using GestionCasos.Domain.Services;
using GestionCasos.Domain.Tests.DataBuilder;
using NSubstitute;

namespace GestionCasos.Domain.Tests.Services;

public class ActualizarSolicitudServiceTests
{
    private readonly IReporteSolicitudService _mockReporteSolicitudService;
    private readonly ActualizarSolicitudService _actualizarSolicitudService;
    public ActualizarSolicitudServiceTests()
    {
        _mockReporteSolicitudService = Substitute.For<IReporteSolicitudService>();
        _actualizarSolicitudService = new(_mockReporteSolicitudService);
    }

    [Fact]
    public async Task ActualizarSolicitudAsync_DebeActualizarLaSolicitud()
    {
        // Arrange
        var solicitudModificada = ObtenerSolicitud();
        var solicitudId = "123";
        var customfield = new Dictionary<string, object>();
        customfield.Add("customfield_10010", "valor del campo personalizado");
        _mockReporteSolicitudService.ActualizarAsync(Arg.Any<string>(), Arg.Any<RequestSolicitudDto>()).Returns(solicitudModificada);

        // Act
        var resultado = await _actualizarSolicitudService.ActualizarSolicitudAsync(customfield, solicitudId);

        // Assert
        Assert.NotEmpty(resultado.Adjunto!.Url!.ToString());

        // Verify
        await _mockReporteSolicitudService.Received(1).ActualizarAsync(solicitudId, Arg.Is<RequestSolicitudDto>(x =>
            x.Fields.Customfields!.ContainsKey("customfield_10010") &&
            (string)x.Fields.Customfields["customfield_10010"] == "valor del campo personalizado"));
    }


    [Fact]
    public void BuildCampoCustom_RetornaUnUnicoDatoConLlaveValor()
    {
        // Arrange
        var expectedKey = "testKey";
        var expectedValue = "testValue";

        // Act
        var result = _actualizarSolicitudService.BuildCampoCustom(expectedKey, expectedValue);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedValue, result[expectedKey]);
    }

    private static Solicitud ObtenerSolicitud()
    {
        string proyectoId = "123";
        string proyectoKey = "KEY123";
        string resumen = "Resumen de la solicitud";
        string solicitudPadre = "AC-123";
        string solicitudId = "123";
        string solicitudKey = "test-123";
        Adjunto adjuntoMock = new AdjuntoTestDataBuilder()
                            .Build();
        TipoSolicitud tipoSolicitudMock = new TipoSolicitudDataBuilder()
        .WithId("1")
        .WithNombre("TipoSolicitud1")
        .Build();

        Solicitud solicitud = new SolicitudDataBuilder()
            .WithProyectoId(proyectoId)
            .WithProyectoKey(proyectoKey)
            .WithSolicitudId(solicitudId)
            .WithSolicitudKey(solicitudKey)
            .WithSolicitudPadreId(solicitudPadre)
            .WithResumen(resumen)
            .WithTipoSolicitud(tipoSolicitudMock)
            .WithOpcionalAdjunto(adjuntoMock)
            .Build();

        return solicitud;
    }
}
