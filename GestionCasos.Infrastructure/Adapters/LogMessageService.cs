using gestion_casos.Infrastructure.Resources;
using GestionCasos.Domain.Ports;

namespace GestionCasos.Infrastructure.Services
{
    public class LogMessageService : ILogMessageService
    {
        public string ErrorPersistencia => ErrorMessages.PERSISTENCIA_EXCEPTION;
        public string ErrorReporteSolicitud => ErrorMessages.REPORTE_SOLICITUD_EXCEPTION;
        public string ErrorValidacionCliente => ErrorMessages.VALIDACION_CLIENTE;
        public string ErrorValidacionConfig => ErrorMessages.VALIDACION_CONFIG;
        public string ErrorValidacionEntidad => ErrorMessages.VALIDACION_ENTIDAD;
        public string ErrorValidacionEntidadId => ErrorMessages.VALIDACION_ENTIDAD_ID;
        public string ErrorValidacionHttp => ErrorMessages.VALIDACION_HTTP;
        public string ErrorValidacionNombreBaseDeDatos => ErrorMessages.VALIDACION_NOMBRE_BASE_DE_DATOS;
        public string ErrorValidacionAdjunto => ErrorMessages.VALIDACION_ADJUNTO;
        public string ErrorValidacionEntidadGuardar => ErrorMessages.VALIDACION_ENTIDAD_GUARDAR;
        public string ErrorValidacionEntidadActualizar => ErrorMessages.VALIDACION_ENTIDAD_ACTUALIZAR;
        public string ErrorValidacionEstadoSolicitud => ErrorMessages.VALIDACION_ESTADO_SOLICITUD;
        public string ErrorValidacionEstadoSolicitudMapper => ErrorMessages.VALIDACION_ESTADO_SOLICITUD_MAPPER;
        public string ContentType => ErrorMessages.APPLICATION_CONTENT_TYPE;
        public string ErrorValidacionLlamadoApi => ErrorMessages.ERROR_LLAMADO_API;
        public string ErrorApiJira => ErrorMessages.ERROR_API_JIRA;
        public string ErrorFormatoDeFechaNoValido => ErrorMessages.ERROR_FORMATO_DE_FECHA_INVALIDO;
        public string ErrorValidacionSolicitudKey => ErrorMessages.VALIDACION_SOLICITUD_KEY;
        public string ErrorValidacionSolicitudEstatus => ErrorMessages.VALIDACION_SOLICITUD_ESTATUS;
        public string ErrorValidacionFechaActualizacionSolicitud => ErrorMessages.VALIDACION_FECHA_ACTUALIZACION_SOLICITUD;
        public string ErrorValidacionActulizar => ErrorMessages.VALIDACION_ERROR_ACTUALIZAR;
        public string ErrorValidacionBaseDeDatos => ErrorMessages.VALIDACION_ERROR_EN_LA_BASE_DE_DATOS;
        public string ErrorValidacionApiKeyNotFound => ErrorMessages.API_KEY_NOT_FOUND;
        public string ErrorValidacionApiKeyInvalid => ErrorMessages.API_KEY_INVALID;
        public string ErrorNotificacionCorreo => ErrorMessages.VALIDACION_NOTIFICACION_CORREO;
        public string NombreIntermediario => SettingMessages.NOMBRE_INTERMEDIARIO;
        public string NumeroSolicitud => SettingMessages.NUMERO_SOLICITUD;
        public string CambioEstado => SettingMessages.CAMBIO_ESTADO;
        public string EstatusSolicitud => SettingMessages.ESTATUS_SOLICITUD;
        public string IdiomaPlantilla => SettingMessages.IDIOMA_PLANTILLA;
        public string CorreoDestino => SettingMessages.CORREO_DESTINO;
        public string NombreProveedor => SettingMessages.NOMBRE_PROVEEDOR;
        public string NombrePlantilla => SettingMessages.NOMBRE_PLANTILLA;
        public string NombrePlataforma => SettingMessages.NOMBRE_PLATAFORMA;
        public string LenguajePlantilla => SettingMessages.LENGUAJE_PLANTILLA;
        public string MetadataPlantilla => SettingMessages.PLANTILLA_METADATA;
        public string ErrorNotificacionSms => ErrorMessages.VALIDACION_NOTIFICACION_SMS;
        public string MensajeSms => SettingMessages.MENSAJE_SMS;
        public string DestinatarioSms => SettingMessages.DESTINATARIO_SMS;
        public string PlantillaNoExiste => ErrorMessages.VALIDACION_EXISTENCIA_PLANTILLA;
        public string ErrorObtencionPlantilla => ErrorMessages.VALIDACION_PLANTILLA_EXCEPTION;
        public string Enlace => SettingMessages.ENLACE;
        public string NombreDestinatario => SettingMessages.NOMBRE_DESTINATARIO;
        public string NumeroSolicitudJira => SettingMessages.NUMERO_SOLICITUD_JIRA;
        public string EstatusCambioSolicitud => SettingMessages.ESTATUS_CAMBIO_SOLICITUD;
        public string EnlaceSolicitud => SettingMessages.ENLACE_SOLICITUD;
        public string Comentario => SettingMessages.COMENTARIO;
        public string Comentario_Jira => SettingMessages.COMENTARIO_JIRA;
        public string Sms => SettingMessages.SMS;
    }
}
