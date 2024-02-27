using GestionCasos.Api.Tests.DataBuilder;
using GestionCasos.Domain.Dtos;
using GestionCasos.Domain.Entities;
using GestionCasos.Domain.Exceptions;
using GestionCasos.Domain.Ports;
using GestionCasos.Domain.Services;
using GestionCasos.Domain.Tests.DataBuilder;
using NSubstitute;

namespace GestionCasos.Domain.Tests.Services;

public class SolicitudServiceTests
{
    private readonly ISolicitudPadreId _mockSolicitudPadreId;
    private readonly IReporteSolicitudService _mockReporteSolicitudService;
    private readonly ISolicitudRepository _mockSolicitudRepository;
    private readonly ILogMessageService _mockResourceMessages;
    private readonly SolicitudService _solicitudService;
    public SolicitudServiceTests()
    {
         _mockSolicitudPadreId = Substitute.For<ISolicitudPadreId>();
         _mockReporteSolicitudService = Substitute.For<IReporteSolicitudService>();
         _mockSolicitudRepository = Substitute.For<ISolicitudRepository>();
         _mockResourceMessages = Substitute.For<ILogMessageService>();
         _solicitudService = new(_mockSolicitudRepository, _mockReporteSolicitudService, _mockSolicitudPadreId,_mockResourceMessages);
    }

    [Fact]
    public async Task GuardarSolicitud_DebeGuardarLaSolicitud_CuandoEsValida()
    {
        // Arrange
        var solicitud = ObtenerSolicitudValida();
        _mockSolicitudRepository.GuardarSolicitudAsync(solicitud).Returns(solicitud);

        // Act
        var esSolicitudValida = solicitud.ValidarSolicitud();
        var resultado = await _solicitudService.GuardarSolicitudAsync(solicitud);

        // Assert
        Assert.True(esSolicitudValida);
        Assert.Equal(solicitud, resultado);

        // Verify
        await _mockSolicitudRepository.Received(1).GuardarSolicitudAsync(solicitud);
    }

    [Fact]
    public async Task GuardarSolicitud_DebeLanzarErrorDePersistenciaExpection_CuandoElRepositorioFalle()
    {
        // Arrange
        var solicitud = ObtenerSolicitudValida();
        _mockSolicitudRepository.GuardarSolicitudAsync(Arg.Any<Solicitud>()).Returns(Task.FromException<Solicitud>(new PersistenciaException("Error")));

        // Act & Assert
        await Assert.ThrowsAsync<PersistenciaException>(() => _solicitudService.GuardarSolicitudAsync(solicitud));

        // Verify
        await _mockSolicitudRepository.Received(1).GuardarSolicitudAsync(solicitud);
    }

    [Fact]
    public async Task GuardarSolicitud_DebeLanzarGestionCasosException_CuandoLaSolicitudNoEsValida()
    {
        // Arrange
        Solicitud solicitud = new SolicitudDataBuilder()
                        .WithTipoSolicitud(new TipoSolicitudDataBuilder().Build())
                        .Build();

        // Act & Assert
        await Assert.ThrowsAsync<GestionCasosException>(() => _solicitudService.GuardarSolicitudAsync(solicitud));

        // Verify
        await _mockSolicitudRepository.Received(0).GuardarSolicitudAsync(solicitud);
    }

    [Fact]
    public async Task CrearSolicitud_DebeEnviarLaSolicitud()
    {
        // Arrange
        var responseSolicitudesCreadasDto = new ResponseSolicitudesCreadasDtoBuilder();
        var responseNuevaSolicitud = new ResponseNuevaSolicitudDtoBuilder();
        var nuevaSolicitudDto = responseNuevaSolicitud.WithId("1").WithKey("ABC-123").Build();
        var responseSolicitudesCreadas = responseSolicitudesCreadasDto
                                    .AddIssue(nuevaSolicitudDto)
                                    .WithIssues([nuevaSolicitudDto])
                                    .Build();
        var solicitudes = new List<RequestSolicitudDto>();
        var projectBuilder = new ProjectBuilder();
        var project = projectBuilder.WithId("1001").WithKey("PRJ").Build();

        var issuetypeBuilder = new IssuetypeBuilder();
        var issuetype = issuetypeBuilder.WithId("5").WithName("Task").Build();

        var fields = new FieldsBuilder()
            .WithProject(project)
            .WithSummary("Este es un resumen")
            .WithIssuetype(issuetype)
            .WithCustomfields(new Dictionary<string, object>
            {
                { "customfield_10010", "valor del campo personalizado" }
            })
            .Build();

        var requestSolicitudDto = new RequestSolicitudDtoBuilder()
                                  .WithFields(fields)
                                  .Build();

        solicitudes.Add(requestSolicitudDto);

        _mockReporteSolicitudService.CrearAsync(solicitudes).Returns(responseSolicitudesCreadas);

        // Act
        var resultado = await _solicitudService.CrearSolicitudesAsync(solicitudes);

        // Assert
        Assert.Equal(responseSolicitudesCreadas.Issues.FirstOrDefault(), resultado.Issues.FirstOrDefault());

        // Verify
        await _mockReporteSolicitudService.Received(1).CrearAsync(solicitudes);
    }

    [Fact]
    public async Task CrearSolicitud_DebeLanzarErrorDeReporteSolicitudException_CuandoElServicioFalle()
    {
        // Arrange

        var requestSolicitud = new List<RequestSolicitudDto>();
        _mockReporteSolicitudService.CrearAsync(Arg.Any<List<RequestSolicitudDto>>())
            .Returns(Task.FromException<ResponseSolicitudesCreadasDto>(new ReporteSolicitudException("Error")));

        // Act & Assert
        await Assert.ThrowsAsync<ReporteSolicitudException>(() => _solicitudService.CrearSolicitudesAsync(requestSolicitud));

        // Verify
        await _mockReporteSolicitudService.Received(1).CrearAsync(Arg.Any<List<RequestSolicitudDto>>());
    }

    [Fact]
    public async Task ConsultarSolicitud_DebeRetornarLaSolicitud()
    {
        // Arrange
        Solicitud solicitud = new SolicitudDataBuilder()
                        .WithTipoSolicitud(new TipoSolicitudDataBuilder().Build())
                        .Build();
        var idorkey = "algun_id_or_key";

        _mockReporteSolicitudService.ConsultarAsync(idorkey).Returns(solicitud);

        // Act
        var resultado = await _solicitudService.ConsultarSolicitudAsync(idorkey);

        // Assert
        Assert.Equal(solicitud, resultado);

        // Verify
        _ = await _mockReporteSolicitudService.Received(1).ConsultarAsync(idorkey)!;
    }

    [Fact]
    public async Task ConsultarSolicitud_DebeLanzarErrorDeReporteSolicitudException_CuandoElServicioFalle()
    {
        // Arrange
        var idorkey = "algun_id_or_key";
        _mockReporteSolicitudService.ConsultarAsync(Arg.Any<string>()).Returns(Task.FromException<Solicitud>(new ReporteSolicitudException("Error")));

        // Act & Assert
        await Assert.ThrowsAsync<ReporteSolicitudException>(() => _solicitudService.ConsultarSolicitudAsync(idorkey));

        // Verify
        _ = await _mockReporteSolicitudService.Received(1).ConsultarAsync(Arg.Any<string>())!;
    }

    [Fact]
    public void ObtenerNombreSolicitudPadreId_DebeRetornarNombreDeSolicitudPadreId()
    {
        // Arrange
        string nombreEsperado = "NombreSolicitudPadre";
        _mockSolicitudPadreId.ObtenerNombre().Returns(nombreEsperado);

        // Act
        string nombreObtenido = _solicitudService.ObtenerNombreSolicitudPadreId();

        // Assert
        Assert.Equal(nombreEsperado, nombreObtenido);

        // Verify
        _mockSolicitudPadreId.Received(1).ObtenerNombre();
    }

    [Fact]
    public async Task ConsultarSolicitud_DebeRetornarNull_CuandoLaSolicitudNoExiste()
    {
        // Arrange
        string idOrKeyInexistente = "id_inexistente";
        _ = _mockReporteSolicitudService.ConsultarAsync(idOrKeyInexistente).Returns(Task.FromResult(new Solicitud()));

        // Act
        var resultado = await _solicitudService.ConsultarSolicitudAsync(idOrKeyInexistente);

        // Assert
        Assert.NotNull(resultado);

        // Verify
        _ = _mockReporteSolicitudService.Received(1).ConsultarAsync(Arg.Any<string>());
    }

    [Fact]
    public void ObtenerNombreSolicitudPadreId_DebeRetornarNull_CuandoElServicioDevuelveNull()
    {
        // Arrange
        _ = _mockSolicitudPadreId.ObtenerNombre();

        // Act
        var nombreObtenido = _solicitudService.ObtenerNombreSolicitudPadreId();

        // Assert
        Assert.NotNull(nombreObtenido);

        // Verify
        _mockSolicitudPadreId.Received(2).ObtenerNombre();
    }

    [Fact]
    public async Task ActualizarSolicitud_DebeActualizarLaSolicitud_CuandoEsValida()
    {
        // Arrange
        var solicitud = ObtenerSolicitudValida();
        _mockSolicitudRepository.ActualizarSolicitudAsync(solicitud).Returns(solicitud);

        // Act
        var esSolicitudValida = solicitud.ValidarSolicitud();
        var resultado = await _solicitudService.ActualizarSolicitudAsync(solicitud);

        // Assert
        Assert.True(esSolicitudValida);
        Assert.Equal(solicitud, resultado);

        // Verify
        await _mockSolicitudRepository.Received(1).ActualizarSolicitudAsync(solicitud);
    }

    [Fact]
    public async Task ActualizarSolicitud_DebeLanzarErrorDePersistenciaExpection_CuandoElRepositorioFalle()
    {
        // Arrange
        var solicitud = ObtenerSolicitudValida();
        _mockSolicitudRepository.ActualizarSolicitudAsync(Arg.Any<Solicitud>()).Returns(Task.FromException<Solicitud>(new PersistenciaException("Error")));

        // Act & Assert
        await Assert.ThrowsAsync<PersistenciaException>(() => _solicitudService.ActualizarSolicitudAsync(solicitud));

        // Verify
        await _mockSolicitudRepository.Received(1).ActualizarSolicitudAsync(solicitud);
    }

    [Fact]
    public async Task ActualizarSolicitud_DebeLanzarGestionCasosException_CuandoLaSolicitudNoEsValida()
    {
        // Arrange
        Solicitud solicitud = new SolicitudDataBuilder()
                        .WithTipoSolicitud(new TipoSolicitudDataBuilder().Build())
                        .Build();

        // Act & Assert
        await Assert.ThrowsAsync<GestionCasosException>(() => _solicitudService.ActualizarSolicitudAsync(solicitud));

        // Verify
        await _mockSolicitudRepository.Received(0).ActualizarSolicitudAsync(solicitud);
    }

    private static Solicitud ObtenerSolicitudValida()
    {
        string proyectoId = "123";
        string proyectoKey = "KEY123";
        string resumen = "Resumen de la solicitud";
        string solicitudPadre = "AC-123";
        string solicitudId= "123";
        string solicitudKey= "test-123";

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
            .Build();

        return solicitud;
    }
}
