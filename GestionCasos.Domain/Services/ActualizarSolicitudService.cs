using GestionCasos.Domain.Dtos;
using GestionCasos.Domain.Entities;
using GestionCasos.Domain.Ports;

namespace GestionCasos.Domain.Services;

[DomainService]
public class ActualizarSolicitudService(IReporteSolicitudService _reporteSolicitudService)
{
    public async Task<Solicitud> ActualizarSolicitudAsync(Dictionary<string, object> camposCustom, string solicitudId)
    {
        var solicitudRequest = new RequestSolicitudDto
        {
            Fields = new Fields
            {
                Customfields = camposCustom
            }
        };
        return await _reporteSolicitudService.ActualizarAsync(solicitudId, solicitudRequest);
    }
    public Dictionary<string, object> BuildCampoCustom(string customFieldId, object valor)
    {
        var campo = new Dictionary<string, object>();
        campo.Add(customFieldId, valor);
        return campo;
    }
}

