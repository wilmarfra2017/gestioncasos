using GestionCasos.Domain.Dtos;

namespace GestionCasos.Domain.Tests.DataBuilder;

public class RequestSolicitudDtoBuilder
{
    private readonly RequestSolicitudDto _requestSolicitudDto = new();

    public RequestSolicitudDtoBuilder WithFields(Fields fields)
    {
        _requestSolicitudDto.Fields = fields;
        return this;
    }

    public RequestSolicitudDto Build()
    {
        return _requestSolicitudDto;
    }
}

public class FieldsBuilder
{
    private readonly Fields _fields = new();

    public FieldsBuilder WithProject(Project project)
    {
        _fields.Project = project;
        return this;
    }

    public FieldsBuilder WithSummary(string summary)
    {
        _fields.Summary = summary;
        return this;
    }

    public FieldsBuilder WithIssuetype(Issuetype issuetype)
    {
        _fields.Issuetype = issuetype;
        return this;
    }

    public FieldsBuilder WithCustomfields(Dictionary<string, object> customfields)
    {
        _fields.Customfields = customfields;
        return this;
    }

    public Fields Build()
    {
        return _fields;
    }
}

public class IssuetypeBuilder
{
    private readonly Issuetype _issuetype = new();

    public IssuetypeBuilder WithId(string id)
    {
        _issuetype.Id = id;
        return this;
    }

    public IssuetypeBuilder WithName(string name)
    {
        _issuetype.Name = name;
        return this;
    }

    public Issuetype Build()
    {
        return _issuetype;
    }
}

public class ProjectBuilder
{
    private readonly Project _project = new();

    public ProjectBuilder WithId(string id)
    {
        _project.Id = id;
        return this;
    }

    public ProjectBuilder WithKey(string key)
    {
        _project.Key = key;
        return this;
    }

    public Project Build()
    {
        return _project;
    }
}
