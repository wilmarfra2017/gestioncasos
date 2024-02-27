using GestionCasos.Domain.Dtos;
using GestionCasos.Domain.Entities;

namespace GestionCasos.Domain.Ports;
public interface ICatalogoRepository
{
    Task<PaginadoDto<Catalogo>> ObtenerCatalogosPaginadoAsync(
                                                                    int pagina,
                                                                    int tamanoPagina,
                                                                    string? nombreCatalogo,
                                                                    string? conClave,
                                                                    string? conValor);
}
