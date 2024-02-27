using GestionCasos.Domain.Entities;

namespace GestionCasos.Domain.Tests.DataBuilder;

public class AdjuntoTestDataBuilder
{
    private readonly Adjunto _adjunto = new();
    public Adjunto Build()
    {
        _adjunto.Url = new Uri("https://localtest.com/container");
        return _adjunto!;
    }
}
