using GestionCasos.Domain.Dtos;

namespace GestionCasos.Infrastructure.Ports;

public interface IJiraService
{
    Task<string> CreateIssueAsync(RequestSolicitudDto issue);
    Task<string> CreateIssueAsync(StringContent jsonString);
    Task<string> UpdateIssueAsync(string keyorid, RequestSolicitudDto issue);
    Task<string> UpdateIssueAsync(string keyorid, StringContent jsonString);

    Task<string> GetIssueAsync(string keyorid);

    Task<string> GetProjectAsync(string keyorid);
    Task<string> AddAttachmentAsync(string keyorid, string documentName, string documentPath);
}
