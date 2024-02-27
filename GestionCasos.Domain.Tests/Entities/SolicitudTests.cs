using GestionCasos.Domain.Entities;
using GestionCasos.Api.Tests.DataBuilder;
using GestionCasos.Domain.Tests.DataBuilder;

namespace GestionCasos.Domain.Tests.Entities;

public class SolicitudTests
{
    [Fact]
    public void ValidarSolicitud_CuandoTodosLosCamposSonValidos_DebeRetornarTrue()
    {
        // Arrange
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

        // Act
        Solicitud solicitud = new SolicitudDataBuilder()
            .WithProyectoId(proyectoId)
            .WithProyectoKey(proyectoKey)
            .WithSolicitudId(solicitudId)
            .WithSolicitudKey(solicitudKey)
            .WithSolicitudPadreId(solicitudPadre)
            .WithResumen(resumen)
            .WithTipoSolicitud(tipoSolicitudMock)
            .Build();
        var solicitudRespuesta = solicitud.ValidarSolicitud();

        // Assert
        Assert.True(solicitudRespuesta);
    }
    [Fact]
    public void ValidarSolicitud_CuandoFaltenAsignacionesEsInvalido_DebeRetornarFalse()
    {
        // Arrange
        string proyectoId = "123";
        string proyectoKey = "KEY123";
        string resumen = "Resumen de la solicitud";

        TipoSolicitud tipoSolicitudMock = new TipoSolicitudDataBuilder()
                .WithId("1")
                .WithNombre("TipoSolicitud1")
                .Build();

        // Act
        Solicitud solicitud = new SolicitudDataBuilder()
            .WithProyectoId(proyectoId)
            .WithProyectoKey(proyectoKey)
            .WithResumen(resumen)
            .WithTipoSolicitud(tipoSolicitudMock)
            .Build();

        var solicitudRespuesta = solicitud.ValidarSolicitud();

        // Assert
        Assert.False(solicitudRespuesta);
    }

    [Fact]
    public void Solicitud_Parametros_Optional()
    {
        // Arrange
        var usuarioAsignado = new UsuarioBuilder()
            .WithId(Guid.NewGuid().ToString())
            .WithNombre($"UsuarioAsignadoTest{Guid.NewGuid()}")
            .Build();

        var usuarioCreacion = new UsuarioBuilder()
            .WithId(Guid.NewGuid().ToString())
            .WithNombre($"UsuarioCreacionTest{Guid.NewGuid()}")
            .Build();

        var usuarioModificacion = new UsuarioBuilder()
            .WithId(Guid.NewGuid().ToString())
            .WithNombre($"UsuarioModificacionTest{Guid.NewGuid()}")
            .Build();

        var adjunto = new AdjuntoTestDataBuilder().Build();

        // Act
        Solicitud solicitud = new SolicitudDataBuilder()
            .WithOpcionalTitulo("TituloTest")
            .WithOpcionalDescripcion("DescripcionTest")
            .WithOpcionalUsuarioAsignado(usuarioAsignado)
            .WithOpcionalUsuarioCreacion(usuarioCreacion)
            .WithOpcionalUsuarioModificacion(usuarioModificacion)
            .WithOpcionalAdjunto(adjunto)
            .Build();

        // Assert
        Assert.Equal("TituloTest", solicitud.Titulo);
        Assert.Equal("DescripcionTest", solicitud.Descripcion);
        Assert.NotNull(solicitud.UsuarioAsignado);
        Assert.NotNull(solicitud.UsuarioCreacion);
        Assert.NotNull(solicitud.UsuarioModificacion);
        Assert.NotNull(solicitud.Adjunto!);
    }
}
