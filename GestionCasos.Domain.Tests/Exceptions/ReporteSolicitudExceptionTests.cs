using GestionCasos.Domain.Exceptions;

namespace GestionCasos.Domain.Tests.Exceptions;

public class ReporteSolicitudExceptionTests
{
    [Fact]
    public void Constructor_WithoutParameters_CreatesInstanceWithDefaultMessage()
    {
        // Arrange & Act
        Assert.NotNull(new ReporteSolicitudException());
    }

    [Fact]
    public void Constructor_WithMessage_CreatesInstanceWithSpecifiedMessage()
    {
        // Arrange
        var expectedMessage = "Custom message";

        // Act
        var exception = new ReporteSolicitudException(expectedMessage);

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
        var exception = new ReporteSolicitudException("Custom message", innerException);

        // Assert
        Assert.NotNull(exception);
        Assert.Equal("Custom message", exception.Message);
        Assert.Equal(innerException, exception.InnerException);
    }
}
