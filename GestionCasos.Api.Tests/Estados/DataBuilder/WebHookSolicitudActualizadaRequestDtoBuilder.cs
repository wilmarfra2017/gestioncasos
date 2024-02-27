using GestionCasos.Domain.Dtos;

namespace GestionCasos.Api.Tests.EstadosSolicitudes.DataBuilder;

public class WebHookSolicitudActualizadaRequestDtoBuilder
{
    private string? key;
    private JiraFields? fields;

    public WebHookSolicitudActualizadaRequestDtoBuilder WithKey(string? key)
    {
        this.key = key;
        return this;
    }

    public WebHookSolicitudActualizadaRequestDtoBuilder WithFields(JiraFields? fields)
    {
        this.fields = fields;
        return this;
    }

    public WebHookSolicitudActualizadaRequestDto Build()
    {
        if (key != null && fields != null)
        {
            return new WebHookSolicitudActualizadaRequestDto(key, fields);
        }
        else
        {
            return new WebHookSolicitudActualizadaRequestDto("123", new JiraFields(new JiraStatus("123", "Pendiente documentacion"), "2024-02-19T06:01:32.167+0000", new JiraComment(["comentario"])));
        }

    }

}

public class JiraFieldsBuilder
{
    private JiraStatus? status;
    private string? updated;
    private JiraComment? comment;

    public JiraFieldsBuilder WithStatus(JiraStatus? status)
    {
        this.status = status;
        return this;
    }

    public JiraFieldsBuilder WithUpdated(string? updated)
    {
        this.updated = updated;
        return this;
    }

    public JiraFieldsBuilder WithComment(JiraComment? comment)
    {
        this.comment = comment;
        return this;
    }

    public JiraFields Build()
    {
        if (status != null && updated != null && comment != null)
        {
            return new JiraFields(status, updated, comment);
        }
        else
        {
            return new JiraFields(new JiraStatus("123", "Pendiente documentacion"), "2024-02-19T06:01:32.167+0000", new JiraComment(["comentario"]));
        }

    }
}


