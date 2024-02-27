namespace GestionCasos.Application.Dtos;
public class ResponsePaginadoDto<T> where T : class
{
    public IEnumerable<T> Registros { get; set; } = default!;
    public int Pagina { get; set; }
    public int TamanoPagina { get; set; }
    public int TotalPaginas { get; set; }
    public double TotalRegistros { get; set; }
}
