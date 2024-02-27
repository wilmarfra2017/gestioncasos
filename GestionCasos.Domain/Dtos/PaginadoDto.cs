namespace GestionCasos.Domain.Dtos;
public class PaginadoDto<T> where T : class
{
    public IEnumerable<T> Data { get; set; } = default!;
    public int TotalPaginas { get; set; }
    public double TotalRegistros { get; set; }
}
