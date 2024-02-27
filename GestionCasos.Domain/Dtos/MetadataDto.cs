namespace GestionCasos.Domain.Dtos;

public class MetadataDto
{
    public string Key { get; set; } = default!;
    public string description { get; set; } = default!;
    public bool isRequired { get; set; } = default!;
    public string Value { get; set; } = default!;
}
