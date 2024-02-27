namespace GestionCasos.Application.Dtos;

public class GenericResponseDto<T>(ResponseDto response) where T : class
{
    public void SetData(T data)
    {
        Data = data;
    }
    public ResponseDto Respuesta { get; } = response;
    public T? Data { get; private set; }

}

public record ResponseDto(
    bool Satisfactorio,
    int Codigo,
    string Mensaje,
    DateTime Fecha
);

