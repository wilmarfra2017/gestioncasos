namespace GestionCasos.Domain.Exceptions;

[Serializable]
public sealed class ReporteSolicitudException : GestionCasosException
{
    public ReporteSolicitudException() : base()
    {
    }

    public ReporteSolicitudException(string msg) : base(msg)
    {
    }

    public ReporteSolicitudException(string message, Exception inner) : base(message, inner)
    {
    }

}
