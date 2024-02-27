namespace GestionCasos.Domain.Exceptions;

[Serializable]
public sealed class AdjuntoInvalidoException : GestionCasosException
{
    public AdjuntoInvalidoException()
    {
    }

    public AdjuntoInvalidoException(string msg) : base(msg)
    {
    }

    public AdjuntoInvalidoException(string message, Exception inner) : base(message, inner)
    {
    }
}

