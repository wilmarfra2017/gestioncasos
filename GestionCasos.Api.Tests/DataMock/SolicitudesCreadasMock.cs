using MongoDB.Bson;

namespace GestionCasos.Api.Tests.DataBuilder;

public static class SolicitudesCreadasMock
{
    public static BsonDocument Obtener()
    {
        return new BsonDocument
            {
                { "ProyectoId", "1111" },
                { "ProyectoKey", "TEST" },
                { "SolicitudPadreId", "TEST-123" },
                { "SolicitudId", "917813" },
                { "SolicitudKey", "TEST-190205" },
                { "Titulo", BsonNull.Value },
                { "Descripcion", BsonNull.Value },
                { "Resumen", "Resumen de la solicitud" },
                { "TipoSolicitud", new BsonDocument
                    {
                        { "Nombre", "TEST" },
                        { "_id", "1" }
                    }
                },
                { "Estatus", new BsonDocument
                    {
                        { "Nombre", "Nuevo" },
                        { "_id", "1" }
                    }
                },
                { "UsuarioAsignado", BsonNull.Value },
                { "UsuarioCreacion", new BsonDocument
                    {
                        { "Nombre", "usert_test" },
                        { "_id", "user123" }
                    }
                },
                { "UsuarioModificacion", BsonNull.Value },
                { "FechaCreacion", new BsonDateTime(DateTime.Now) },
                { "FechaModificacion", new BsonDateTime(DateTime.Now)},
                { "Comentario", BsonNull.Value}
            };
    }
}