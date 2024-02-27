namespace GestionCasos.Domain.Entities;
public class PlantillaApiRespuesta
{
    public TemplateResponse Response { get; set; } = default!;
    public Pagination Pagination { get; set; } = default!;
    public List<TemplateData> Data { get; set; } = default!;
}

public class TemplateResponse
{
    public bool Success { get; set; }
    public int Code { get; set; }
    public string Message { get; set; } = default!;
    public DateTime Date { get; set; }
}

public class Pagination
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int PageCount { get; set; }
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }
}

public class TemplateData
{
    public string Name { get; set; } = default!;
    public string Content { get; set; } = default!;
}