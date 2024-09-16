using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace UserService.Entity.Model;


public class RolePermission
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string? RoleName { get; set; }
    public string? Table { get; set; }
    public List<string>? Permission { get; set; }
}