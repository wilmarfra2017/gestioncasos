using GestionCasos.Domain.Dtos;
using GestionCasos.Domain.Entities;
using GestionCasos.Domain.Ports;
using GestionCasos.Domain.Exceptions;
using GestionCasos.Infrastructure.Dtos;
using GestionCasos.Infrastructure.Helpers;
using GestionCasos.Infrastructure.Ports;
using System.Globalization;
using System.Text.Json;

namespace GestionCasos.Infrastructure.Adapters
{
    [Repository]
    public class ReporteSolicitudService(IJiraService jiraService, ILogMessageService resourceManager) : IReporteSolicitudService
    {
        readonly IJiraService _jiraService = jiraService ?? throw new ArgumentNullException(nameof(jiraService));
        private readonly ILogMessageService _resourceManager = resourceManager ?? throw new ArgumentNullException(nameof(resourceManager));
        private readonly JsonSerializerOptions serializeOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        public async Task<ResponseSolicitudesCreadasDto> CrearAsync(IList<RequestSolicitudDto> solicitudes)
        {
            var content = DynamicObjectHelper.CovertirDinamicamenteAStringContent(solicitudes);
            var guardadoRespuesta = await _jiraService.CreateIssueAsync(content);
            var respuesta = JsonSerializer.Deserialize<ResponseSolicitudesCreadasDto>(guardadoRespuesta, serializeOptions)!;
            return respuesta;
        }
        public async Task<Solicitud> ActualizarAsync(string solicitudIdOrKey, RequestSolicitudDto solicitud)
        {
            var content = DynamicObjectHelper.CovertirDinamicamenteAStringContent(solicitud);
            var guardadoRespuesta = await _jiraService.UpdateIssueAsync(solicitudIdOrKey, content);
            if (!string.IsNullOrEmpty(guardadoRespuesta))
            {
                throw new GestionCasosException($"{resourceManager.ErrorValidacionEntidadActualizar} {guardadoRespuesta}");
            }
            return await ConsultarAsync(solicitudIdOrKey);
        }
        public async Task<Solicitud> ConsultarAsync(string keyorid)
        {
            var solicitudRepuesta = await _jiraService.GetIssueAsync(keyorid);
            if (solicitudRepuesta is null)
            {
                throw new ReporteSolicitudException(_resourceManager.ErrorReporteSolicitud);
            }
            var respuesta = JsonSerializer.Deserialize<ReporteSolicitudDto>(solicitudRepuesta, serializeOptions)!;

            if (respuesta.Id is null)
            {
                return new Solicitud();
            }

            var solicitud = new Solicitud
            {
                ProyectoKey = respuesta.Fields.Project.Key,
                ProyectoId = respuesta.Fields.Project.Id,
                SolicitudId = respuesta.Id,
                SolicitudKey = respuesta.Key,
                Resumen = respuesta.Fields.Summary,
                Descripcion = respuesta.Fields.Description,
                TipoSolicitud = new TipoSolicitud(respuesta.Fields.Issuetype.Id, respuesta.Fields.Issuetype.Name),
                Estatus = new Estatus(respuesta.Fields.Status.Id, respuesta.Fields.Status.Name),
                UsuarioAsignado = respuesta.Fields.Assignee != null ? new Usuario(respuesta.Fields.Assignee.Key, respuesta.Fields.Assignee.Name) : null,
                UsuarioCreacion = respuesta.Fields.Creator != null ? new Usuario(respuesta.Fields.Creator.Key, respuesta.Fields.Creator.Name) : null,
                UsuarioModificacion = respuesta.Fields.Reporter != null ? new Usuario(respuesta.Fields.Reporter.Key, respuesta.Fields.Reporter.Name) : null,
                FechaCreacion = DateTime.Parse(respuesta.Fields.Created, CultureInfo.InvariantCulture),
                FechaModificacion = DateTime.Parse(respuesta.Fields.Updated, CultureInfo.InvariantCulture)
            };

            return solicitud;
        }
    }
}
