namespace GestionCasos.Domain.Dtos;

public record WebHookSolicitudActualizadaRequestDto(string Key, JiraFields Fields);
public record JiraFields(JiraStatus Status, string Updated, JiraComment? Comment);
public record JiraStatus(string Id, string Name);
public record JiraComment(string[]? Comments);
