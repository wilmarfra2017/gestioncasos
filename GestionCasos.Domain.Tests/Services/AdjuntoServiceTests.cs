using GestionCasos.Domain.Exceptions;
using GestionCasos.Domain.Ports;
using GestionCasos.Domain.Services;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System.Text;

namespace GestionCasos.Domain.Tests.Services;

public class AdjuntoServiceTests
{
    [Theory]
    [InlineData("application/json")]
    [InlineData("image/gif")]
    [InlineData("text/javascript")]
    public async Task CargarAdjuntoAsync_WithInvalidFormatFile_ThrowsAdjuntoInvalidoException(string fileFormat)
    {
        // Arrange
        IAdjuntoRepository adjuntoRespository = Substitute.For<IAdjuntoRepository>();
        AdjuntoService adjuntoService = new(adjuntoRespository, null);
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes("Some file"));
        var fileName = "Mi archivo de test";
        string formatoArchivo = fileFormat;
        string solicitudId = "333";

        // Act & Assert
        await Assert.ThrowsAsync<AdjuntoInvalidoException>(async () =>
        {
            _ = await adjuntoService.CargarAdjuntoAsync(fileName, stream, formatoArchivo, solicitudId);
        });

        // Verify
        await adjuntoRespository.Received(0).CargarAdjuntoAsync(Arg.Any<Stream>(), Arg.Any<string>(), Arg.Any<string>());
    }

    [Fact]
    public async Task CargarAdjuntoAsync_WithInvalidStreamData_ThrowsAdjuntoInvalidoException()
    {
        // Arrange
        Stream? stream = null;
        IAdjuntoRepository adjuntoRespository = Substitute.For<IAdjuntoRepository>();
        AdjuntoService adjuntoService = new(adjuntoRespository, null);
        var fileName = "Mi archivo de test";
        string formatoArchivo = "application/pdf";
        string solicitudId = "333";

        // Act & Assert
        await Assert.ThrowsAsync<AdjuntoInvalidoException>(async () =>
        {
            _ = await adjuntoService.CargarAdjuntoAsync(fileName, stream!, formatoArchivo, solicitudId);
        });

        // Verify
        await adjuntoRespository.Received(0).CargarAdjuntoAsync(Arg.Any<Stream>(), Arg.Any<string>(), Arg.Any<string>());
    }

    [Fact]
    public async Task CargarAdjuntoAsync_WithInvalidBlobStorageResponse_ThrowsArgumentNullException()
    {
        // Arrange
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes("Some file"));
        var fileName = "Mi archivo de test";
        IAdjuntoRepository adjuntoRespository = Substitute.For<IAdjuntoRepository>();
        AdjuntoService adjuntoService = new(adjuntoRespository, null);
        string formatoArchivo = "image/jpeg";
        string solicitudId = "333";
        adjuntoRespository.CargarAdjuntoAsync(Arg.Any<Stream>(), Arg.Any<string>(), Arg.Any<string>()).ReturnsNull();

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
        {
            _ = await adjuntoService.CargarAdjuntoAsync(fileName, stream, formatoArchivo, solicitudId);
        });

        // Verify
        await adjuntoRespository.Received(1).CargarAdjuntoAsync(Arg.Any<Stream>(), Arg.Any<string>(), Arg.Any<string>());
    }

    [Theory]
    [InlineData("application/pdf")]
    [InlineData("application/msword")]
    [InlineData("application/vnd.openxmlformats-officedocument.wordprocessingml.document")]
    [InlineData("image/jpeg")]
    [InlineData("image/png")]
    public async Task CargarAdjuntoAsync_Success_ShouldReturnUrl(string fileFormat)
    {
        // Arrange
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes("Some file"));
        var fileName = "Mi archivo de test";
        IAdjuntoRepository adjuntoRespository = Substitute.For<IAdjuntoRepository>();
        AdjuntoService adjuntoService = new(adjuntoRespository, null);
        string formatoArchivo = fileFormat;
        string solicitudId = "333";
        string url = "https://mail.google.com/";
        adjuntoRespository.CargarAdjuntoAsync(Arg.Any<Stream>(), Arg.Any<string>(), Arg.Any<string>()).Returns(new Uri(url));

        // Act
        var result = await adjuntoService.CargarAdjuntoAsync(fileName, stream, formatoArchivo, solicitudId);

        // Assert
        Assert.Equal(url, result.Url!.ToString());

        // Verify
        await adjuntoRespository.Received(1).CargarAdjuntoAsync(Arg.Any<Stream>(), Arg.Any<string>(), Arg.Any<string>());
    }

    [Theory]
    [InlineData("application/pdf")]
    [InlineData("application/msword")]
    [InlineData("application/vnd.openxmlformats-officedocument.wordprocessingml.document")]
    [InlineData("image/jpeg")]
    [InlineData("image/png")]
    public async Task CargarAdjuntoAsync_Success_Adjunto(string fileFormat)
    {
        // Arrange
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes("Some file"));
        var fileName = "Mi archivo de test";
        IAdjuntoRepository adjuntoRespository = Substitute.For<IAdjuntoRepository>();
        AdjuntoService adjuntoService = new(adjuntoRespository, null);
        string solicitudId = Guid.NewGuid().ToString();
        string url = $"https://test.humano.local/api/v1/gestion-casos/{solicitudId}/adjunto";
        adjuntoRespository.CargarAdjuntoAsync(Arg.Any<Stream>(), Arg.Any<string>(), Arg.Any<string>()).Returns(new Uri(url));

        // Act
        var result = await adjuntoService.CargarAdjuntoAsync(fileName, stream, fileFormat, solicitudId);

        // Assert
        Assert.Equal(url, result.Url!.ToString());

        // Verify
        await adjuntoRespository.Received(1).CargarAdjuntoAsync(Arg.Any<Stream>(), Arg.Any<string>(), Arg.Any<string>());
    }
}
