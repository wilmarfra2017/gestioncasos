using MongoDB.Bson;

namespace GestionCasos.Api.Tests.DataMock;

public static class CatalogosCreadosMock
{
    public static BsonDocument Obtener()
    {
        return new BsonDocument
            {
                { "Nombre", "Seguros" },
                { "Descripcion", "Documentos requeridos según el tipo movimiento" },
                { "Elementos", new BsonArray
                    {
                        new BsonDocument
                        {
                            { "Clave", "Inclusión de dependiente" },
                            { "Valor", "Requerido" }
                        },
                        new BsonDocument
                        {
                            { "Clave", "Exclusión de titular" },
                            { "Valor", "N/A" }
                        }
                    }
                }
            };
    }
}