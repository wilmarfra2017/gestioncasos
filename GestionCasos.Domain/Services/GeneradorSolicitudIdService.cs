using GestionCasos.Domain.Utils.GeneradorId;
using System.Globalization;

namespace GestionCasos.Domain.Services;

[DomainService]
public class GeneradorSolicitudIdService
{
    private readonly IGeneradorNumeroAleatorio _generadorNumero;
    private const int VALOR_MIN = 100;
    private const int VALOR_MAX = 999;
    private const string FORMATO_FECHA = "ddMMyyHHmmssffffff";
    public GeneradorSolicitudIdService(IGeneradorNumeroAleatorio generadorNumero)
    {
        _generadorNumero = generadorNumero ?? throw new ArgumentNullException(nameof(generadorNumero));
    }

    public string GenerarId(string prefijoSolicitud)
    {
        int randomNum = _generadorNumero.Generar(VALOR_MIN, VALOR_MAX);
        string timestamp = DateTime.Now.ToString(FORMATO_FECHA, CultureInfo.InvariantCulture);
        string solicitudId = $"{prefijoSolicitud}-{timestamp}{randomNum}";
        return solicitudId;
    }
}
