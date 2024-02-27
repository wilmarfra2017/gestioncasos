using Microsoft.AspNetCore.Mvc.Filters;

namespace GestionCasos.Api.Filters;

[AttributeUsage(AttributeTargets.All)]
public sealed class ValidateAttribute : ExceptionFilterAttribute
{

}
