namespace GestionCasos.Infrastructure.Config;

public class NotificacionConfig
{
    public Uri Host { get; set; } = default!;
    public string EnpointEmail { get; set; } = default!;
    public string EnpointSms { get; set; } = default!;
    public string RutaPrefijo { get; set; } = default!;
    public int MaximoReintentos { get; set; } = default!;
    public double FrecuenciaReintentos { get; set; } = default!;
}
