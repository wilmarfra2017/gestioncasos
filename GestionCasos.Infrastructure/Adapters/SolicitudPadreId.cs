using GestionCasos.Domain.Ports;
using GestionCasos.Infrastructure.Config;

namespace GestionCasos.Infrastructure.Adapters;
[Repository]
public class SolicitudPadreId : ISolicitudPadreId
{
    public string ObtenerNombre()
    {
        return JiraCamposConfig.SolicitudPadreId;
    }
}
