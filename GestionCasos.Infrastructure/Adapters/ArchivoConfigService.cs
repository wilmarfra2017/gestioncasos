using GestionCasos.Domain.Ports;
using GestionCasos.Infrastructure.Config;

namespace GestionCasos.Infrastructure.Adapters;

[Repository]
public class ArchivoConfigService : IArchivoConfigService
{
    public int ObtenerPesoMaximo()
    {
        return ArchivoConfig.PesoMax;
    }
}
