using GestionCasos.Domain.Exceptions;

namespace GestionCasos.Domain.Tests.Exceptions;

public class AdjuntoInvalidoExceptionTests
{
    [Fact]
    public void Constructor_WithoutParameters_CreatesInstanceWithDefaultMessage()
    {
        // Arrange & Act
        AdjuntoInvalidoException exception = new();

        // Assert
        Assert.NotNull(exception);
    }

    [Fact]
    public void Constructor_WithMessage_CreatesInstanceWithMessage()
    {
        // Arrange
        string expectedMessage = "Test message";

        // Act
        AdjuntoInvalidoException exception = new(expectedMessage);

        // Assert
        Assert.Equal(expectedMessage, exception.Message);
    }

    [Fact]
    public void Constructor_WithInnerException_CreatesInstanceWithInnerException()
    {
        // Arrange
        string expectedMessage = "Test message";
        var innerException = new Exception("Inner exception");

        // Act
        AdjuntoInvalidoException exception = new(expectedMessage, innerException);

        // Assert
        Assert.Equal(expectedMessage, exception.Message);
        Assert.Equal(innerException, exception.InnerException);
    }
}
