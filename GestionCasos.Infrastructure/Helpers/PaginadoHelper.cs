namespace GestionCasos.Infrastructure.Helpers;

public static class PaginadoHelper
{
    public static int CalcularSkip(int pagina, int tamanoPagina)
    {
        return pagina <= 1 ? 0 : (pagina - 1) * tamanoPagina;
    }

    public static int CalcularTotalPaginas(double totalRegistros, int tamanoPagina)
    {
        return (int)Math.Ceiling(totalRegistros / tamanoPagina);
    }
}
