using Newtonsoft.Json;

namespace GestionCasos.Infrastructure.Extensions;

public static class JsonExtensions
{
    public static bool TryDeserializeJson<T>(this string data, out T? dataDeserialized) where T : new()
    {
        dataDeserialized = default;

        if (string.IsNullOrEmpty(data))
        {
            return false;
        }

        try
        {
            dataDeserialized = JsonConvert.DeserializeObject<T>(data);
            return true;
        }
        catch (JsonException)
        {
            return false;
        }
    }
}

