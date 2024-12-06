using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MovieService.Core.Entity.Model;

public class BaseModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
}