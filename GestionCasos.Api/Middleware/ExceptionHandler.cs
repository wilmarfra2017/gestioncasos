using GestionCasos.Api.Dtos;
using GestionCasos.Application.Dtos;
using GestionCasos.Domain.Exceptions;
using GestionCasos.Infrastructure.Config;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Immutable;
using System.Globalization;
using System.Net;
using System.Net.Mime;
using System.Text;

namespace GestionCasos.Api.Middleware;

public class AppExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<AppExceptionHandlerMiddleware> _logger;
    private readonly ImmutableArray<string> METHODS_HAS_BODY_REQUEST = [HttpMethods.Post, HttpMethods.Put];
    private readonly ImmutableArray<string> LOG_CAMPOS;
    private readonly HttpConfig _httpConfig;
    private readonly JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
    {
        ContractResolver = new CamelCasePropertyNamesContractResolver()
    };
    public AppExceptionHandlerMiddleware(RequestDelegate next,
            ILogger<AppExceptionHandlerMiddleware> logger,
            HttpConfig httpConfig)
    {
        _next = next;
        _logger = logger;
        _httpConfig = httpConfig;
        LOG_CAMPOS = _httpConfig.LoggingCampos;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var loggerDto = new LoggerDto();
        var originalBody = context.Response.Body;
        try
        {
            context.Request.EnableBuffering();
            await FillRequestLoggerDtoAsync(loggerDto, context);
            using var memoryStream = new MemoryStream();
            context.Response.Body = memoryStream;

            await _next(context);

            memoryStream.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(memoryStream);
            string responseBody = await reader.ReadToEndAsync();

            var responseObject = JsonConvert.DeserializeObject<dynamic>(responseBody, jsonSerializerSettings);
            string statusCodeName = Enum.GetName(typeof(HttpStatusCode), context.Response.StatusCode) ?? context.Response.StatusCode.ToString(CultureInfo.InvariantCulture);

            var response = new ResponseDto(
                Satisfactorio: true,
                Codigo: context.Response.StatusCode,
                Mensaje: statusCodeName,
                Fecha: DateTime.Now
            );
            GenericResponseDto<dynamic> responseDto = new(response);
            responseDto.SetData(responseObject);

            string modifiedResponseBody = JsonConvert.SerializeObject(responseDto, jsonSerializerSettings);

            FillResponseLoggerDto(loggerDto, responseBody);

            byte[] byteArray = Encoding.UTF8.GetBytes(modifiedResponseBody);
            await originalBody.WriteAsync(byteArray);
        }
        catch (GestionCasosException ex)
        {
            loggerDto.AgregarDato("error", ex.Message);

            var statusCode = DetermineStatusCode(ex);

            var response = new ResponseDto(
                Satisfactorio: false,
                Codigo: statusCode,
                Mensaje: ex.Message,
                Fecha: DateTime.Now
            );
            var result = JsonConvert.SerializeObject(new GenericResponseDto<String>(response), jsonSerializerSettings);

            context.Response.ContentType = MediaTypeNames.Application.Json;
            context.Response.StatusCode = statusCode;
            byte[] byteArray = Encoding.UTF8.GetBytes(result);
            await originalBody.WriteAsync(byteArray);
        }
        finally
        {
            _logger.LogInformation("@{loggerDto.Accion} @{loggerDto.Fecha} @{loggerDto.ProyectoKey} @{loggerDto.Datos}",
                                    loggerDto.Accion,
                                    LoggerDto.Fecha,
                                    loggerDto.ProyectoKey,
                                    loggerDto.Datos);
        }
    }
    private static int DetermineStatusCode(Exception ex)
    {
        return (ex is PersistenciaException || ex is ReporteSolicitudException)
            ? (int)HttpStatusCode.InternalServerError
            : (int)HttpStatusCode.BadRequest;
    }
    private async Task FillRequestLoggerDtoAsync(LoggerDto loggerDto, HttpContext context)
    {
        string method = context.Request.Method;
        loggerDto.Accion = string.Format(CultureInfo.InvariantCulture,"{0}: {1}", method, context.Request.Path);
        loggerDto.ProyectoKey = context.Request.Headers[_httpConfig.ProyectoHeaderName];

        if (context.Request.Body.CanRead && METHODS_HAS_BODY_REQUEST.Contains(method) && context.Request.ContentType!.Contains(MediaTypeNames.Application.Json, StringComparison.OrdinalIgnoreCase))
        {
            using var reader = new StreamReader(
                context.Request.Body,
                Encoding.UTF8,
                detectEncodingFromByteOrderMarks: false,
                bufferSize: 512, leaveOpen: true
            );

            var requestBody = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;

            var datos = LoggerHandler.GetBodyData(requestBody, LOG_CAMPOS);
            loggerDto.EstablecerDatos(datos);

        }
    }

    private void FillResponseLoggerDto(LoggerDto loggerDto, string responseBody)
    {
        var responseDataLogger = LoggerHandler.GetBodyData(responseBody, LOG_CAMPOS);
        if (responseDataLogger is null || responseDataLogger.Count <= 0)
        {
            return;
        }

        foreach (var item in responseDataLogger)
        {
            loggerDto.AgregarDato(item.Key, item.Value);
        }
    }
}

