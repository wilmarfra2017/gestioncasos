namespace GestionCasos.Api.Dtos;

public class LoggerDto
{
    private Dictionary<string, object>? _datos;

    public string? ProyectoKey { get; set; }
    public string? Accion { get; set; }
    public IReadOnlyDictionary<string, object>? Datos => _datos;
    public static DateTime Fecha { get { return DateTime.Now; } }

    public LoggerDto()
    {
        _datos = new Dictionary<string, object>();
    }

    public void AgregarDato(string clave, object valor)
    {
        _datos ??= new Dictionary<string, object>();
        _datos[clave] = valor;
    }

    public void EstablecerDatos(Dictionary<string, object> nuevosDatos)
    {
        _datos = nuevosDatos;
    }
}
