using GestionCasos.Domain.Dtos;
namespace GestionCasos.Domain.Ports;

public interface INotificacionApiSmsService
{
    Task<bool> EnviarSmsNotificacionAsync(NotificacionSmsRequestDto notificacionSmsRequestDto);
}
