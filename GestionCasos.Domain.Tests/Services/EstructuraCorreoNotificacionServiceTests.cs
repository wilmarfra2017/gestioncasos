using gestion_casos.Domain.Tests.DataBuilder;
using GestionCasos.Domain.Entities;
using GestionCasos.Domain.Ports;
using GestionCasos.Domain.Services;
using GestionCasos.Domain.Tests.DataBuilder;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace GestionCasos.Domain.Tests.Services;

public class EstructuraCorreoNotificacionServiceTests
{
    private readonly IEstructuraCorreoService _estructuraCorreoNotificacionService;
    private readonly IOptions<ConfiguracionNotificacion> _mockConfiguracionNotificacionOptions;
    private readonly ILogMessageService _mockResourceManager;
    private readonly DominioHumano _dominioHumano;

    public EstructuraCorreoNotificacionServiceTests()
    {
        _mockConfiguracionNotificacionOptions = Substitute.For<IOptions<ConfiguracionNotificacion>>();
        _mockResourceManager = Substitute.For<ILogMessageService>();
        _dominioHumano = new DominioHumano { InternalMailDomain = "example.com" };

        var configuracionNotificacion = new ConfiguracionNotificacion
        {
            DomainJira = "http://jira.example.com/",
            NameTemplateEmail = "NombrePlantilla",
            PlatformName = "PlataformaTest",
            InternalProvider = "ProveedorInterno",
            ExternalProvider = "ProveedorExterno"
        };
        _mockConfiguracionNotificacionOptions.Value.Returns(configuracionNotificacion);

        _estructuraCorreoNotificacionService = new EstructuraCorreoNotificacionService(
            _mockConfiguracionNotificacionOptions,
            _dominioHumano,
            _mockResourceManager
        );
    }


    [Fact]
    public async Task CrearEstructuraCorreo_DebeRetornarEstructuraValida()
    {
        // Arrange
        var configuracionNotificacion = new ConfiguracionNotificacionCorreoBuilder()
            .ConDominioJira("http://jira.example.com/")
            .ConNombrePlantillaEmail("NombrePlantilla")
            .ConNombrePlataforma("PlataformaTest")
            .ConProveedorInterno("ProveedorInterno")
            .ConProveedorExterno("ProveedorExterno")
            .Build();

        _mockConfiguracionNotificacionOptions.Value.Returns(configuracionNotificacion);

        ConfigurarResourceManager();

        var email = "test@example.com";
        var comentario = "Este es un comentario";
        var estado = "Pendiente";
        var nombreIntermediario = "Intermediario Test";
        var solicitudKey = "SOL-123";

        // Act
        var resultado = await _estructuraCorreoNotificacionService.CrearEstructuraCorreoAsync(email, comentario, estado, nombreIntermediario, solicitudKey);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(email, resultado.CorreoDestinatario);
        Assert.Equal("NombrePlantilla", resultado.Plantilla.Nombre);
        Assert.Equal("PlataformaTest", resultado.Plantilla.NombrePlataforma);
        Assert.Contains(resultado.Plantilla.Metadatos, m => m.Key == "nombreIntermediario" && m.Value == nombreIntermediario);
    }

    [Fact]
    public async Task CrearEstructuraCorreo_CuandoDebeUsarProveedorInterno()
    {
        // Arrange
        var emailInterno = "test@" + _dominioHumano.InternalMailDomain;
        var comentario = "Este es un comentario para un correo interno";
        var estado = "Revisado";
        var nombreIntermediario = "Intermediario Interno";
        var solicitudKey = "INT-456";

        ConfigurarResourceManager();

        // Act
        var resultado = await _estructuraCorreoNotificacionService.CrearEstructuraCorreoAsync(emailInterno, comentario, estado, nombreIntermediario, solicitudKey);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(emailInterno, resultado.CorreoDestinatario);
        Assert.Equal(_mockConfiguracionNotificacionOptions.Value.InternalProvider, resultado.NombreProveedor);
    }

    [Fact]
    public async Task CrearEstructuraCorreo_DebeAsignarMetadatosCorrectamente()
    {
        // Arrange
        var email = "usuario@externo.com";
        var comentario = "Este es un comentario";
        var estado = "Pendiente";
        var nombreIntermediario = "Intermediario Test";
        var solicitudKey = "SOL-123";

        ConfigurarMocks();

        // Act
        var resultado = await _estructuraCorreoNotificacionService.CrearEstructuraCorreoAsync(email, comentario, estado, nombreIntermediario, solicitudKey);

        // Assert
        Assert.NotNull(resultado);
        Assert.NotEmpty(resultado.Plantilla.Metadatos);

        var metadatoNombreIntermediario = resultado.Plantilla.Metadatos.Find(m => m.Key == _mockResourceManager.NombreIntermediario);
        Assert.NotNull(metadatoNombreIntermediario);
        Assert.Equal(nombreIntermediario, metadatoNombreIntermediario.Value);

        var metadatoNumeroSolicitud = resultado.Plantilla.Metadatos.Find(m => m.Key == _mockResourceManager.NumeroSolicitud);
        Assert.NotNull(metadatoNumeroSolicitud);
        Assert.Equal(solicitudKey, metadatoNumeroSolicitud.Value);

        var metadatoEstatusSolicitud = resultado.Plantilla.Metadatos.Find(m => m.Key == _mockResourceManager.EstatusSolicitud);
        Assert.NotNull(metadatoEstatusSolicitud);
        Assert.Equal(estado, metadatoEstatusSolicitud.Value);

        var metadatoEnlace = resultado.Plantilla.Metadatos.Find(m => m.Key == _mockResourceManager.Enlace);
        Assert.NotNull(metadatoEnlace);
        Assert.Equal(_mockConfiguracionNotificacionOptions.Value.DomainJira + solicitudKey, metadatoEnlace.Value);

        var metadatoComentario = resultado.Plantilla.Metadatos.Find(m => m.Key == _mockResourceManager.Comentario);
        Assert.NotNull(metadatoComentario);
        Assert.Equal(comentario, metadatoComentario.Value);
    }

    private void ConfigurarMocks()
    {
        _mockConfiguracionNotificacionOptions.Value.Returns(new ConfiguracionNotificacion
        {
            DomainJira = "http://jira.example.com/",
            NameTemplateEmail = "NombrePlantilla",
            PlatformName = "PlataformaTest",
            InternalProvider = "ProveedorInterno",
            ExternalProvider = "ProveedorExterno"
        });

        _mockResourceManager.NombreIntermediario.Returns("nombreIntermediario");
        _mockResourceManager.NumeroSolicitud.Returns("numeroSolicitud");
        _mockResourceManager.EstatusSolicitud.Returns("estatusSolicitud");
        _mockResourceManager.Enlace.Returns("enlace");
        _mockResourceManager.Comentario.Returns("comentario");
        _mockResourceManager.IdiomaPlantilla.Returns("es");
    }


    private void ConfigurarResourceManager()
    {
        _mockResourceManager.NombreIntermediario.Returns("nombreIntermediario");
        _mockResourceManager.NumeroSolicitud.Returns("numeroSolicitud");
        _mockResourceManager.EstatusSolicitud.Returns("estatusSolicitud");
        _mockResourceManager.Enlace.Returns("enlace");
        _mockResourceManager.Comentario.Returns("comentario");
        _mockResourceManager.IdiomaPlantilla.Returns("es");
    }
}