namespace GestionCasos.Domain.Ports
{
    public interface ILogMessageService
    {
        string ErrorPersistencia { get; }
        string ErrorReporteSolicitud { get; }
        string ErrorValidacionCliente { get; }
        string ErrorValidacionConfig { get; }
        string ErrorValidacionEntidad { get; }
        string ErrorValidacionEntidadId { get; }
        string ErrorValidacionHttp { get; }
        string ErrorValidacionNombreBaseDeDatos { get; }
        string ErrorValidacionAdjunto { get; }
        string ErrorValidacionEntidadGuardar { get; }
        string ErrorValidacionEntidadActualizar { get; }
        string ErrorValidacionEstadoSolicitud { get; }
        string ErrorValidacionEstadoSolicitudMapper { get; }
        string ContentType { get; }
        string ErrorValidacionLlamadoApi { get; }
        string ErrorApiJira { get; }
        string ErrorFormatoDeFechaNoValido { get; }
        string ErrorValidacionSolicitudKey { get; }
        string ErrorValidacionSolicitudEstatus { get; }
        string ErrorValidacionFechaActualizacionSolicitud { get; }
        string ErrorValidacionActulizar { get; }
        string ErrorValidacionBaseDeDatos { get; }
        string ErrorValidacionApiKeyNotFound { get; }
        string ErrorValidacionApiKeyInvalid { get; }
        string ErrorNotificacionCorreo { get; }
        string ErrorNotificacionSms { get; }
        string NombreIntermediario { get; }
        string NumeroSolicitud { get; }
        string CambioEstado { get; }
        string EstatusSolicitud { get; }
        string IdiomaPlantilla { get; }
        string CorreoDestino { get; }
        string NombreProveedor { get; }
        string NombrePlantilla { get; }
        string NombrePlataforma { get; }
        string LenguajePlantilla { get; }
        string MetadataPlantilla { get; }
        string MensajeSms { get; }
        string DestinatarioSms { get; }
        string PlantillaNoExiste { get; }
        string ErrorObtencionPlantilla { get; }
        string Enlace { get; }
        string NombreDestinatario { get; }
        string NumeroSolicitudJira { get; }
        string EstatusCambioSolicitud { get; }
        string EnlaceSolicitud { get; }
        string Comentario { get; }
        string Comentario_Jira { get; }
        string Sms { get; }
    }
}
