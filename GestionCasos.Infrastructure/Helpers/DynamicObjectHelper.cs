using GestionCasos.Domain.Dtos;
using System.Dynamic;
using System.Globalization;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GestionCasos.Infrastructure.Helpers;

public static class DynamicObjectHelper
{
    private static readonly JsonSerializerOptions serializeOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        WriteIndented = true
    };
    public static StringContent CovertirDinamicamenteAStringContent(IList<RequestSolicitudDto> solicitudes)
    {
        var listaSolicitudesJson = new List<object>();
        if (solicitudes == null)
        {
            throw new ArgumentNullException(nameof(solicitudes), "La lista de solicitudes no puede ser nula.");
        }
        foreach (var solicitud in solicitudes)
        {
            var solicitudJson = CovertirSolicitudAObject(solicitud);
            listaSolicitudesJson.Add(solicitudJson);
        }
        string jsonSolicitudes = JsonSerializer.Serialize(new
        {
            issueUpdates = listaSolicitudesJson
        }, serializeOptions);

        var stringContent = new StringContent(jsonSolicitudes, Encoding.UTF8, MediaTypeNames.Application.Json);
        return stringContent;
    }
    public static StringContent CovertirDinamicamenteAStringContent(RequestSolicitudDto solicitud)
    {
        var expandoObject = new ExpandoObject();
        var solicitudJson = CovertirDeCustomfieldsAObject(solicitud.Fields.Customfields!,expandoObject);
        string jsonSolicitudes = JsonSerializer.Serialize(solicitudJson, serializeOptions);
        var stringContent = new StringContent(jsonSolicitudes, Encoding.UTF8, MediaTypeNames.Application.Json);
        return stringContent;
    }
    private static object CovertirSolicitudAObject(RequestSolicitudDto solicitud)
    {
        var expandoObject = new ExpandoObject();
        CovertirDeCustomfieldsAObject(solicitud.Fields.Customfields!,expandoObject);
        CovertirDeFieldsAObject(solicitud.Fields!,expandoObject);
        return new { fields = expandoObject };
    }
    private static object CovertirDeCustomfieldsAObject(Dictionary<string, object> customfields, ExpandoObject expandoObject)
    {
        if (customfields is not null)
        {
            foreach (var entry in customfields!)
            {
                ((IDictionary<string, object>)expandoObject!).Add(entry.Key, entry.Value);
            }
        }  
        return new { fields = expandoObject };
    }
    private static void CovertirDeFieldsAObject(Fields? fields, ExpandoObject expandoObject)
    {
        var propertiesToInclude = ObtenerNombrePropiedades(fields!);
        foreach (var propertyName in propertiesToInclude)
        {
            var propertyInfo = fields?.GetType().GetProperty(propertyName);
            if (propertyInfo != null)
            {
                var value = propertyInfo.GetValue(fields);
                expandoObject.TryAdd(propertyName.ToLower(CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture), value);
            }
        }
    }

    private static List<string> ObtenerNombrePropiedades(Fields request)
    {
        List<string> listaPropiedades = [];
        if (request is null)
        {
            return listaPropiedades;
        }

        Type t = typeof(Fields);
        PropertyInfo[] propiedades = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (PropertyInfo propiedad in propiedades)
        {
            string nombrePropiedad = propiedad.Name;

            if (!propiedad.PropertyType.IsGenericType && !string.IsNullOrEmpty(nombrePropiedad))
            {
                listaPropiedades.Add(nombrePropiedad);
            }
        }

        return listaPropiedades;
    }
}

