using GestionCasos.Domain.Dtos;

namespace GestionCasos.Domain.Entities;

public class Plantilla
{
    public string Nombre { get; set; } = default!;
    public string NombrePlataforma { get; set; } = default!;
    public List<MetadataDto> Metadatos { get; set; } = default!;
    public string Idioma { get; set; } = default!;
}

