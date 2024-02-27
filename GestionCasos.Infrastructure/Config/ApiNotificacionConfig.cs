namespace gestion_casos.Infrastructure.Config;

public class ApiNotificacionConfig
{
    public Uri URL { get; set; } = default!;
    public string EmailSendUrl { get; set; } = default!;
    public string SmsSendUrl { get; set; } = default!;
    public string TemplateUrl { get; set; } = default!;
}
