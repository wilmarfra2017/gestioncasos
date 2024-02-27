using System.Globalization;

namespace gestion_casos.Domain.Utils.GeneradorId;

public static class FechaUtil
{
    private static readonly string[] FormatosFecha = [
       "dd/MM/yyyy",
        "MM/dd/yyyy",
        "yyyy-MM-dd",
        "dd-MM-yyyy",
        "MM-dd-yyyy",
        "yyyyMMdd",
        "ddMMyyyy",
        "d/M/yyyy",
        "M/d/yyyy",
        "yyyy/MM/dd",
        "yyyy-MM-ddTHH:mm:ss",
        "yyyy-MM-dd HH:mm:ss",
        "dd-MMM-yyyy",
        "yyyy-MM-ddTHH:mm:ss.FFFFFFFzzz"
    ];

    public static bool TryParseFechaExacta(string fechaEntrada, out DateTime fechaConvertida)
    {
        return DateTime.TryParseExact(fechaEntrada, FormatosFecha, CultureInfo.InvariantCulture, DateTimeStyles.None, out fechaConvertida);
    }
}

