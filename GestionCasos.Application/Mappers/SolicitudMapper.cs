using GestionCasos.Domain.Dtos;
using GestionCasos.Domain.Entities;
using GestionCasos.Application.Dtos;
using GestionCasos.Application.Features.Solicitudes.Commands.CrearSolicitud;
using GestionCasos.Domain.Exceptions;
namespace GestionCasos.Application.Mappers;
public static class SolicitudMapper
{
    public static ResponsePaginadoDto<ObtenerSolicitudesDto> ToResponsePaginadoDto(PaginadoDto<Solicitud> paginadoSolicitudes, int pagina, int tamanoPagina)
    {
        ArgumentNullException.ThrowIfNull(paginadoSolicitudes);
        var solicitudesDto = paginadoSolicitudes.Data.Select(c => c.ToObtenerSolicitudesDto()).ToList();
        return new ResponsePaginadoDto<ObtenerSolicitudesDto>
        {
            Registros = solicitudesDto,
            Pagina = pagina,
            TamanoPagina = tamanoPagina,
            TotalPaginas = paginadoSolicitudes.TotalPaginas,
            TotalRegistros = paginadoSolicitudes.TotalRegistros
        };
    }
    public static ObtenerSolicitudesDto ToObtenerSolicitudesDto(this Solicitud solicitudEntity)
    {
        ArgumentNullException.ThrowIfNull(solicitudEntity);
        return new ObtenerSolicitudesDto(solicitudEntity.ProyectoKey,
                                        solicitudEntity.SolicitudPadreId,
                                        solicitudEntity.SolicitudId,
                                        solicitudEntity.SolicitudKey,
                                        solicitudEntity.Resumen,
                                        solicitudEntity.FechaCreacion,
                                        solicitudEntity.FechaModificacion,
                                        solicitudEntity.TipoSolicitud.ToTipoSolicitudDto(),
                                        solicitudEntity.Estatus.ToEstatusDto(),
                                        solicitudEntity.UsuarioCreacion?.ToUsuarioDto(),
                                        solicitudEntity.Descripcion
                                        );
    }
    public static ResponseCrearSolicitudDto ToResponseCrearSolicitudDto(ResponseSolicitudesCreadasDto responseDto)
    {
        ArgumentNullException.ThrowIfNull(responseDto);
        var solicitudesDto = responseDto.Issues.Select(issue => new SolicitudDto(issue.Id, issue.Key)).ToList();
        return new ResponseCrearSolicitudDto(responseDto.SolicitudIdExterna, solicitudesDto);
    }

    public static IEnumerable<RequestSolicitudDto> ToRequestSolicitudDtos(CrearSolicitudesCommand command, string nombreCampoPadre, string solicitudId)
    {
        ArgumentNullException.ThrowIfNull(command);
        return command.Solicitudes.Select(solicitud => solicitud.ToRequestSolicitudDto(nombreCampoPadre, solicitudId)).ToList();
    }
    public static TipoSolicitudDto ToTipoSolicitudDto(this GestionCasos.Domain.Entities.TipoSolicitud tipoSolicitud) 
    {    
        ArgumentNullException.ThrowIfNull(tipoSolicitud);
        return new TipoSolicitudDto(tipoSolicitud.Id, tipoSolicitud.Nombre);
    }
    public static EstatusDto ToEstatusDto(this Estatus estatus) 
    {
        ArgumentNullException.ThrowIfNull(estatus);
        return new EstatusDto(estatus.Id, estatus.Nombre);
    }
    public static UsuarioDto ToUsuarioDto(this Usuario usuario) 
    {
        ArgumentNullException.ThrowIfNull(usuario);
        return new UsuarioDto(usuario.Id, usuario.Nombre);
    }

    private static RequestSolicitudDto ToRequestSolicitudDto(this NuevaSolicitud solicitud, string nombreCampoPadre, string solicitudId)
    {
        AddCampo(solicitud.Campos, nombreCampoPadre, solicitudId);
        return new RequestSolicitudDto
        {
            Fields = new Fields
            {
                Summary = solicitud.Resumen,
                Project = new Project { Key = solicitud.ProjectKey },
                Issuetype = new Issuetype { Name = solicitud.TipoSolicitud.Nombre, Id = solicitud.TipoSolicitud.Id },
                Customfields = solicitud.Campos
            }
        };
    }

    private static void AddCampo(Dictionary<string, object> campos, string nombreCampoPadre, string solicitudId)
    {
        if (campos.ContainsKey(nombreCampoPadre))
        {
            throw new GestionCasosException($"El campo '{nombreCampoPadre}' ya está siendo utilizado.");
        }

        campos[nombreCampoPadre] = solicitudId;
    }
}




