using GestionCasos.Domain.Entities;

namespace GestionCasos.Api.Dtos
{
    public record EstadosSolicitudRequestDto(string IssueKey, DateTime IssueDate, Estatus IssueStatus, string IssueComment);
}
