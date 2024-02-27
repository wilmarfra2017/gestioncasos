
namespace GestionCasos.Infrastructure.Config;
public class JiraConfig
{
    public Uri Url { get; set; } = default!;
    public string Username { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string Base_Endpoint { get; set; } = default!;
}
