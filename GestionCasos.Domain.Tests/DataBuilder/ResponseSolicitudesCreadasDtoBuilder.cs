using GestionCasos.Domain.Dtos;

namespace GestionCasos.Domain.Tests.DataBuilder;
public class ResponseSolicitudesCreadasDtoBuilder
{
    private readonly ResponseSolicitudesCreadasDto _response = new();

    public ResponseSolicitudesCreadasDtoBuilder WithIssues(List<ResponseNuevaSolicitudDto> issues)
    {
        _response.Issues = issues;
        return this;
    }

    public ResponseSolicitudesCreadasDtoBuilder AddIssue(ResponseNuevaSolicitudDto issue)
    {
        if (_response.Issues == null)
        {
            _response.Issues = new List<ResponseNuevaSolicitudDto>();
        }

        _response.Issues.Add(issue);
        return this;
    }

    public ResponseSolicitudesCreadasDto Build()
    {
        return _response;
    }
}