using GestionCasos.Domain.Entities;
using GestionCasos.Domain.Exceptions;
using GestionCasos.Domain.Tests.DataBuilder;

namespace GestionCasos.Domain.Tests.Entities;

public class AdjuntoTests
{
    [Fact]
    public void VerificarStreamData_ThrowsExceptionWhenStreamDataIsNull()
    {
        // Arrange
        Stream? streamData = null;

        // Act & Assert
        Assert.Throws<AdjuntoInvalidoException>(() => Adjunto.VerificarStreamData(streamData!));
    }

    [Theory]
    [InlineData("application/pdf")]
    [InlineData("application/msword")]
    [InlineData("application/vnd.openxmlformats-officedocument.wordprocessingml.document")]
    [InlineData("image/jpeg")]
    [InlineData("image/png")]
    public void VerificarFormatoValido_ValidFormats_DoesNotThrowException(string formato)
    {
        // Arrange
        Adjunto adjunto = new AdjuntoTestDataBuilder().Build();

        // Act & Assert
        Assert.Null(Record.Exception(() => adjunto.VerificarFormatoValido(formato)));
    }

    [Theory]
    [InlineData("application/xml")]
    public void VerificarFormatoValido_InvalidFormat_ThrowsException(string formato)
    {
        // Arrange
        Adjunto adjunto = new AdjuntoTestDataBuilder().Build();

        // Act & Assert
        Assert.Throws<AdjuntoInvalidoException>(() => adjunto.VerificarFormatoValido(formato));
    }

    [Fact]
    public void SetUrl_ThrowsArgumentNullExceptionWhenUrlIsNull()
    {
        // Arrange
        Adjunto adjunto = new AdjuntoTestDataBuilder().Build();
        Uri? uri = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => adjunto.SetUrl(uri!));
    }

    [Fact]
    public void SetUrl_SetsUrlPropertyCorrectly()
    {
        // Arrange
        Adjunto adjunto = new AdjuntoTestDataBuilder().Build();
        Guid solicitudId = Guid.NewGuid();
        var url = new Uri($"https://example.com/api/v1/gestion-casos/{solicitudId}/adjuntos");

        // Act
        adjunto.SetUrl(url);

        // Assert
        Assert.Equal(url.ToString(), adjunto.Url!.ToString());
    }
}
