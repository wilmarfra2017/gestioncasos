using GestionCasos.Domain.Utils.GeneradorId;

namespace GestionCasos.Domain.Tests.DataBuilder;

public static class GeneradorNumeroAleatorioBuilder
{
    public static GeneradorNumeroAleatorio Build()
    {
        return new GeneradorNumeroAleatorio();
    }
}
