namespace GestionCasos.Domain.Dtos;

public class RequestSolicitudDto
{
    public Fields Fields { get; set; } = default!;
}

public class Fields
{
    public Project Project { get; set; } = default!;
    public string Summary { get; set; } = default!;
    public Issuetype Issuetype { get; set; } = default!;
    public Dictionary<string, object>? Customfields { get; set; }

}

public class Issuetype
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
}

public class Project
{
    public string Id { get; set; } = default!;
    public string Key { get; set; } = default!;
}
