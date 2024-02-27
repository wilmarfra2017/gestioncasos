using GestionCasos.Domain.Tests.DataBuilder;
using GestionCasos.Domain.Utils.GeneradorId;
using NSubstitute;

namespace GestionCasos.Domain.Tests.Services;

public class GeneradorSolicitudIdServiceTests
{
    [Fact]
    public void GenerarId_DebeGenerarIdConPrefijoYNumeroAleatorio()
    {
        // Arrange
        string prefijoSolicitud = "PrefijoHumano";
        string expectedIdFormat = $"{prefijoSolicitud}-";
        IGeneradorNumeroAleatorio generadorNumeroMock = Substitute.For<IGeneradorNumeroAleatorio>();
        generadorNumeroMock.Generar(Arg.Any<int>(), Arg.Any<int>()).Returns(123);
        var generadorService = new GeneradorSolicitudIdServiceBuilder()
            .WithGeneradorNumero(generadorNumeroMock)
            .Build();

        // Act
        string generatedId = generadorService.GenerarId(prefijoSolicitud);

        // Assert
        Assert.StartsWith(expectedIdFormat, generatedId);

        // Verify
        generadorNumeroMock.Received(1).Generar(Arg.Any<int>(), Arg.Any<int>());
    }

    [Fact]
    public void GenerarId_DebeLanzarArgumentNullException_SiGeneradorNumeroEsNulo()
    {
        // Arrange
        IGeneradorNumeroAleatorio? generadorNumeroMock = null;

        // Act & Assert
        _ = Assert.Throws<ArgumentNullException>(() => new GeneradorSolicitudIdServiceBuilder()
            .WithGeneradorNumero(generadorNumeroMock!)
            .Build());
    }
}
