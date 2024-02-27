using System.Collections.Immutable;

namespace GestionCasos.Infrastructure.Config;
public class HttpConfig
{
    public string ProyectoHeaderName { get; init; }
    public string PlataformaHeaderName { get; init; }
    public ImmutableArray<string> LoggingCampos { get; init; }

    public HttpConfig(string proyectoHeaderName, string plataformaHeaderName, ImmutableArray<string> loggingCampos)
    {
        ProyectoHeaderName = proyectoHeaderName ?? throw new ArgumentNullException(nameof(proyectoHeaderName));
        PlataformaHeaderName = plataformaHeaderName ?? throw new ArgumentNullException(nameof(plataformaHeaderName));
        LoggingCampos = loggingCampos;
    }
}

