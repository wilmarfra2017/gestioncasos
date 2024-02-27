using GestionCasos.Domain.Dtos;

namespace GestionCasos.Domain.Tests.DataBuilder;
public class ResponseNuevaSolicitudDtoBuilder
{
    private readonly ResponseNuevaSolicitudDto _issue = new();

    public ResponseNuevaSolicitudDtoBuilder WithId(string id)
    {
        _issue.Id = id;
        return this;
    }

    public ResponseNuevaSolicitudDtoBuilder WithKey(string key)
    {
        _issue.Key = key;
        return this;
    }

    public ResponseNuevaSolicitudDto Build()
    {
        return _issue;
    }
}