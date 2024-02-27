using GestionCasos.Infrastructure.Extensions;
using System.Collections.Immutable;

namespace GestionCasos.Api.Middleware;

public static class LoggerHandler
{
    public static Dictionary<string, object> GetBodyData(string body, ImmutableArray<string> fields)
    {
        Dictionary<string, object> datosDictionary = [];

        var listDeserialized = TryDeserializeToList(body);
        
        if (listDeserialized is not null && listDeserialized.Count > 0)
        {
            listDeserialized.ForEach(item => item.FillOutputDictionary(ref datosDictionary, fields));
            return datosDictionary;
        }

        var dictDeserialized = TryDeserializeToDict(body);

        if (dictDeserialized is not null && dictDeserialized.Count > 0)
        {
            dictDeserialized.FillOutputDictionary(ref datosDictionary, fields);
        }

        return datosDictionary;
    }

    private static void FillOutputDictionary(this Dictionary<string, object> inputDictionaty, ref Dictionary<string, object> outputDictionary, ImmutableArray<string> fields)
    {
        foreach (var field in fields)
        {
            object fieldObject = default!;
            if (inputDictionaty is null || !inputDictionaty.TryGetValue(field, out fieldObject!))
            {
                continue;
            }

            var deserializers = new Dictionary<Type, Func<string, object?>>
            {
                { typeof(List<Dictionary<string, object>>), jsonString => jsonString.TryDeserializeJson<List<Dictionary<string, object>>>(out var result) ? result : null },
                { typeof(Dictionary<string, object>), jsonString => jsonString.TryDeserializeJson<Dictionary<string, object>>(out var result) ? result : null },
                { typeof(List<object>), jsonString => jsonString.TryDeserializeJson<List<object>>(out var result) ? result : null },
                { typeof(string), jsonString => jsonString }
            };

            var dataType = deserializers.Keys.FirstOrDefault(type => deserializers[type](fieldObject?.ToString()!) != null);
            var deserializedObject = deserializers[dataType!](fieldObject?.ToString()!);

            outputDictionary.AddOrUpdateDictionaryValue(field, deserializedObject!);
        }
    }

    private static void AddOrUpdateDictionaryValue(this Dictionary<string, object> outputDictionary, string field, object newValue)
    {
        if (outputDictionary.TryGetValue(field, out var fieldValue))
        {
            var newObjectValue = new[]
            {
                fieldValue,
                newValue
            };

            outputDictionary.Remove(field);
            outputDictionary.Add(field, newObjectValue);
        }
        else
        {
            outputDictionary.Add(field, newValue);
        }
    }

    private static List<Dictionary<string, object>>? TryDeserializeToList(string body)
    {
        if (body.TryDeserializeJson<List<Dictionary<string, object>>>(out var result))
        {
            return result;
        }
        return new List<Dictionary<string, object>>();
    }

    private static Dictionary<string, object>? TryDeserializeToDict(string body)
    {
        if (body.TryDeserializeJson<Dictionary<string, object>>(out var result))
        {
            return result;
        }
        return new Dictionary<string, object>();
    }
}
