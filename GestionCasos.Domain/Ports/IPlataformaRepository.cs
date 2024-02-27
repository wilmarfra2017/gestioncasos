using GestionCasos.Domain.Entities;

namespace GestionCasos.Domain.Ports;

public interface IPlataformaRepository
{
    Task<Plataforma> BuscarPorKeyAsync(string key);
}
