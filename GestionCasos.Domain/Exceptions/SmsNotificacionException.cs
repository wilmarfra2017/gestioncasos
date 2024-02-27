namespace gestion_casos.Domain.Exceptions;

public class SmsNotificacionException : Exception
{
    public SmsNotificacionException() : base("Error en el proceso de notificación por SMS.")
    {
    }

    public SmsNotificacionException(string message) : base(message)
    {
    }

    public SmsNotificacionException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
