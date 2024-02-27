using GestionCasos.Domain.Exceptions;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text.RegularExpressions;

namespace GestionCasos.Api.Filters;

public class BasePathFilter : IDocumentFilter
{
    private readonly string basePath;

    public BasePathFilter(string basePath)
    {
        this.basePath = basePath;
    }

    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        _ = swaggerDoc ?? throw new GestionCasosException();

        swaggerDoc.Servers.Add(new() { Url = basePath });
        var pathsToModify = swaggerDoc.Paths
            .Where(keyPair => keyPair.Key.StartsWith(basePath, StringComparison.OrdinalIgnoreCase))
            .ToList();

        foreach (var (path, newKey) in from path in pathsToModify
                                       where path.Key.StartsWith(basePath, StringComparison.OrdinalIgnoreCase)
                                       let newKey = Regex.Replace(path.Key, $"^{basePath}", string.Empty, RegexOptions.IgnoreCase)
                                       select (path, newKey))
        {
            swaggerDoc.Paths.Remove(path.Key);
            swaggerDoc.Paths.Add(newKey, path.Value);
        }
    }
}
