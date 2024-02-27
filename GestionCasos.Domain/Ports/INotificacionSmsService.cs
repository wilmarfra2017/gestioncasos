using GestionCasos.Domain.Dtos;
using GestionCasos.Domain.Entities;
namespace GestionCasos.Domain.Ports;

public interface INotificacionSmsService
{
    Task<bool> EnviarSmsNotificacionAsync(EstructuraSmsNotificacion notificacionSmsRequestDto);
}
