namespace GestionCasos.Domain.Exceptions;


public sealed class PersistenciaException : GestionCasosException
{
    public PersistenciaException() : base()
    {
    }

    public PersistenciaException(string msg) : base(msg)
    {
    }

    public PersistenciaException(string message, Exception inner) : base(message, inner)
    {
    }

}
