using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GestionCasos.Domain.Entities;

public class DomainEntity
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = default!;
}


