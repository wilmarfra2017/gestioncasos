namespace GestionCasos.Api.Tests.Utils;

public static class JsonOptions
{
    public static readonly JsonSerializerOptions DefaultJsonSerializerOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };
}
