using GestionCasos.Domain.Entities;

namespace GestionCasos.Domain.Ports;

public interface INotificacionEmailService
{
    Task<bool> EnviarCorreoNotificacionAsync(EstructuraCorreoNotificacion estructuraNotificacion);
}