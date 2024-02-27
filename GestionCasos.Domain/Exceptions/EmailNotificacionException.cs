namespace gestion_casos.Domain.Exceptions;

public class EmailNotificacionException : Exception
{
    public EmailNotificacionException() : base("Error en el proceso de notificación por correo.")
    {
    }

    public EmailNotificacionException(string message) : base(message)
    {
    }

    public EmailNotificacionException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
