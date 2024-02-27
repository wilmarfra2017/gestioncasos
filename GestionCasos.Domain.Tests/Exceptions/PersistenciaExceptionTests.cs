using GestionCasos.Domain.Exceptions;

namespace GestionCasos.Domain.Tests.Exceptions;

public class PersistenciaExceptionTests
{
    [Fact]
    public void Constructor_WithoutParameters_CreatesInstanceWithDefaultMessage()
    {
        // Arrange & Act
        var exception = new PersistenciaException();

        // Assert
        Assert.NotNull(exception);
    }

    [Fact]
    public void Constructor_WithMessage_CreatesInstanceWithSpecifiedMessage()
    {
        // Arrange
        var expectedMessage = "Custom message";

        // Act
        var exception = new PersistenciaException(expectedMessage);

        // Assert
        Assert.NotNull(exception);
        Assert.Equal(expectedMessage, exception.Message);
    }

    [Fact]
    public void Constructor_WithInnerException_CreatesInstanceWithInnerException()
    {
        // Arrange
        var innerException = new Exception("Inner exception");

        // Act
        var exception = new PersistenciaException("Custom message", innerException);

        // Assert
        Assert.NotNull(exception);
        Assert.Equal("Custom message", exception.Message);
        Assert.Equal(innerException, exception.InnerException);
    }
}
