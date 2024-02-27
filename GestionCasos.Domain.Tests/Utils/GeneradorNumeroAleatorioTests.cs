using GestionCasos.Domain.Tests.DataBuilder;

namespace GestionCasos.Domain.Tests.Utils;

public class GeneradorNumeroAleatorioTests
{
    [Fact]
    public void Generar_DebeGenerarNumeroAleatorioEnRango()
    {
        // Arrange
        int minValue = 1;
        int maxValue = 10;
        var generador = GeneradorNumeroAleatorioBuilder.Build();

        // Act
        int numeroAleatorio = generador.Generar(minValue, maxValue);

        // Assert
        Assert.InRange(numeroAleatorio, minValue, maxValue);
    }
}

