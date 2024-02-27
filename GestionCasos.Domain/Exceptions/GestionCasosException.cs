using System.Runtime.Serialization;

namespace GestionCasos.Domain.Exceptions;

public class GestionCasosException : Exception
{
    private readonly StreamingContext context;

    public GestionCasosException()
    {

    }

    public GestionCasosException(string message) : base(message)
    {
    }

    public GestionCasosException(string message, Exception inner) : base(message, inner)
    {
    }

    public GestionCasosException(string message, StreamingContext context) : this(message)
    {
        this.context = context;
    }
}
