namespace GestionCasos.Domain.Entities;

public class ConfiguracionNotificacion
{
    public string PlatformName { get; set; } = default!;
    public string InternalProvider { get; set; } = default!;
    public string ExternalProvider { get; set; } = default!;
    public string EmailSendUrl { get; set; } = default!;
    public string SmsSendUrl { get; set; } = default!;
    public string TemplateUrl { get; set; } = default!;
    public string URL { get; set; } = default!;
    public string HeaderName { get; set; } = default!;
    public string Key { get; set; } = default!;
    public string NameTemplateEmail { get; set; } = default!;    
    public string NameTemplateSms { get; set; } = default!;    
    public string DomainJira { get; set; } = default!;    
    public string SmsProvider { get; set; } = default!;    
}
