using GestionCasos.Domain.Entities;

namespace GestionCasos.Domain.Ports;
public interface IPlantillaService
{
    Task<TemplateResponse> ObtenerPlantillaAsync(string nombrePlantilla, string nombrePlataforma);
}
