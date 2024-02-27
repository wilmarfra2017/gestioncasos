using GestionCasos.Domain.Services;
using GestionCasos.Domain.Utils.GeneradorId;

namespace GestionCasos.Domain.Tests.DataBuilder;

public class GeneradorSolicitudIdServiceBuilder
{
    private IGeneradorNumeroAleatorio? _generadorNumero;

    public GeneradorSolicitudIdServiceBuilder WithGeneradorNumero(IGeneradorNumeroAleatorio generadorNumero)
    {
        _generadorNumero = generadorNumero;
        return this;
    }

    public GeneradorSolicitudIdService Build()
    {
        return new GeneradorSolicitudIdService(_generadorNumero!);
    }
}
