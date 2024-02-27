namespace GestionCasos.Infrastructure.Dtos;

public class RequestReportDto
{
    public Fields Fields { get; set; } = default!;
}

public class Assignee
{
    public string Key { get; set; } = default!;
    public string Name { get; set; } = default!;
}
public class Creator
{
    public string Key { get; set; } = default!;
    public string Name { get; set; } = default!;
}

public class Component
{
    public string Id { get; set; } = default!;
}

public class Fields
{
    public Project Project { get; set; } = default!;
    public string Summary { get; set; } = default!;
    public Issuetype Issuetype { get; set; } = default!;
    public Assignee? Assignee { get; set; }
    public Creator Creator { get; set; } = default!;
    public Reporter Reporter { get; set; } = default!;
    public Priority Priority { get; set; } = default!;
    public IList<string> Labels { get; set; } = default!;
    public Timetracking Timetracking { get; set; } = default!;
    public SecurityInfo SecurityInfo { get; set; } = default!;
    public IList<Version> Versions { get; set; } = default!;
    public string Environment { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string Duedate { get; set; } = default!;
    public IList<FixVersion> FixVersions { get; set; } = default!;
    public IList<Component> Components { get; set; } = default!;
    public string Customfield12407 { get; set; } = default!;
    public Status Status { get; set; } = default!;
    public string Created { get; set; } = default!;
    public string Updated { get; set; } = default!;
}

public class FixVersion
{
    public string Id { get; set; } = default!;
}

public class Issuetype
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
}


public class Priority
{
    public string Id { get; set; } = default!;
}

public class Project
{
    public string Id { get; set; } = default!;
    public string Key { get; set; } = default!;
}

public class Reporter
{
    public string Key { get; set; } = default!;
    public string Name { get; set; } = default!;
}


public class SecurityInfo
{
    public string Id { get; set; } = default!;
}

public class Timetracking
{
    public string OriginalEstimate { get; set; } = default!;
    public string RemainingEstimate { get; set; } = default!;
}


public class Version
{
    public string Id { get; set; } = default!;
}

public class Status
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
}

