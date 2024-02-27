using gestion_casos.Domain.Tests.DataBuilder;
using GestionCasos.Domain.Entities;
using GestionCasos.Domain.Ports;
using GestionCasos.Domain.Services;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace GestionCasos.Domain.Tests.Services;

public class EstructuraSmsNotificacionServiceTests
{
    private readonly IEstructuraSmsService _estructuraSmsNotificacionService;
    private readonly IOptions<ConfiguracionNotificacion> _mockConfiguracionNotificacionOptions;
    private readonly ILogMessageService _mockResourceManager;

    public EstructuraSmsNotificacionServiceTests()
    {
        _mockConfiguracionNotificacionOptions = Substitute.For<IOptions<ConfiguracionNotificacion>>();
        _mockResourceManager = Substitute.For<ILogMessageService>();

        var configuracionNotificacion = new ConfiguracionNotificacion
        {
            DomainJira = "http://jira.example.com/",
            NameTemplateEmail = "NombrePlantilla",
            PlatformName = "PlataformaTest",
            NameTemplateSms = "PlantillaTest"
        };
        _mockConfiguracionNotificacionOptions.Value.Returns(configuracionNotificacion);

        _estructuraSmsNotificacionService = new EstructuraSmsNotificacionService(
            _mockConfiguracionNotificacionOptions,
            _mockResourceManager
        );
    }

    [Fact]
    public async Task CrearEstructuraSms_DebeRetornarEstructuraValida()
    {
        // Arrange
        var configuracionNotificacion = new ConfiguracionNotificacionSmsBuilder()
            .ConDominioJira("http://jira.example.com/")
            .ConNombrePlantillaSms("PlantillaTest")
            .ConNombrePlataforma("PlataformaTest")
            .ConNombreProveedor("ProveedorTest")
            .Build();

        _mockConfiguracionNotificacionOptions.Value.Returns(configuracionNotificacion);

        ConfigurarResourceManager();

        var numero = "8091234567";
        var comentario = "Este es un comentario";
        var estado = "Pendiente";
        var nombreIntermediario = "Intermediario Test";
        var solicitudKey = "SOL-123";

        // Act
        var resultado = await _estructuraSmsNotificacionService.CrearEstructuraSmsAsync(numero, comentario, estado, nombreIntermediario, solicitudKey);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal("PlantillaTest", resultado.Plantilla.Nombre);
        Assert.Equal("PlataformaTest", resultado.Plantilla.NombrePlataforma);
        Assert.Contains(resultado.Plantilla.Metadatos, m => m.Key == "nombreIntermediario" && m.Value == nombreIntermediario);
    }

    [Fact]
    public async Task CrearEstructuraSms_DebeAsignarMetadatosCorrectamente()
    {
        // Arrange
        var numero = "8091234567";
        var comentario = "Este es un comentario";
        var estado = "Pendiente";
        var nombreIntermediario = "Intermediario Test";
        var solicitudKey = "SOL-123";

        ConfigurarMocks();

        // Act
        var resultado = await _estructuraSmsNotificacionService.CrearEstructuraSmsAsync(numero, comentario, estado, nombreIntermediario, solicitudKey);

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

    [Fact]
    public async Task CrearEstructuraSms_ConValoresInvalidos_DebeManejarErrores()
    {
        // Arrange
        var numeroInvalido = "";
        var comentarioInvalido = (string)null!;
        var estadoInvalido = "";
        var nombreIntermediarioInvalido = (string)null!;
        var solicitudKeyInvalida = "";

        ConfigurarMocks();

        // Act
        var resultado = await _estructuraSmsNotificacionService.CrearEstructuraSmsAsync(numeroInvalido, comentarioInvalido,
            estadoInvalido, nombreIntermediarioInvalido, solicitudKeyInvalida);

        // Assert
        Assert.NotNull(resultado);
        Assert.Contains(resultado.Plantilla.Metadatos, m => m.Key == _mockResourceManager.NombreIntermediario && m.Value == nombreIntermediarioInvalido);
        Assert.Contains(resultado.Plantilla.Metadatos, m => m.Key == _mockResourceManager.NumeroSolicitud && m.Value == solicitudKeyInvalida);
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
}
