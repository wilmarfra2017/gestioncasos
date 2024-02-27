using GestionCasos.Infrastructure.Config;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Globalization;

namespace GestionCasos.Api.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = true)]
public sealed class ApiControllerBaseAttribute : Attribute, IControllerModelConvention
{
    public void Apply(ControllerModel controller)
    {
        string controllerName = ToLowerControllerName(controller.ControllerName);
        string route = string.Format(CultureInfo.InvariantCulture, ApiConfig.PrefixUrl + "/{0}", controllerName);

        controller.Selectors[0].AttributeRouteModel = new()
        {
            Template = route
        };
    }
    private static string ToLowerControllerName(string controllerName)
    {
        if (string.IsNullOrEmpty(controllerName) || controllerName.Length <= 1)
        {
            return controllerName;
        }

        return char.ToLowerInvariant(controllerName[0]) + controllerName.Substring(1);
    }
}

