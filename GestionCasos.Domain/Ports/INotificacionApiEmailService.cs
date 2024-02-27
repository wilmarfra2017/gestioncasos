using GestionCasos.Domain.Dtos;

namespace GestionCasos.Domain.Ports;

public interface INotificacionApiEmailService
{
    Task<bool> EnviarCorreoNotificacionAsync(NotificacionEmailRequestDto notificacionEmailRequestDto);
}

