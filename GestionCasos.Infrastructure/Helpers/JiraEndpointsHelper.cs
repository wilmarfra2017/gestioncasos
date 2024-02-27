namespace GestionCasos.Infrastructure.Helpers;

public static class JiraEndpointsHelper
{
    public static Uri GetIssueUrl(Uri url, string queryParameter) => new($"{url}issue/{queryParameter}");
    public static Uri GetProjectInfoUrl(Uri url, string queryParameter) => new($"{url}project/{queryParameter}");
    public static Uri CreateIssueUrl(Uri url) => new($"{url}issue/bulk");
    public static Uri AddAttachmentUrl(Uri url, string queryParameter) => new($"{url}issue/{queryParameter}/attachments");
}
