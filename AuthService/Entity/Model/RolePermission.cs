using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AuthService.Entity.Model;

public class RolePermission
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string? RoleName { get; set; }
    public List<Permission> Permissions { get; set; }
}

public class Permission
{
    public string? Resource { get; set; }
    public List<string>? Actions { get; set; }
}