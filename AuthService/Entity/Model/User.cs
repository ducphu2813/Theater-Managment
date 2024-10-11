using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AuthService.Entity.Model;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? Email { get; set; }
    public List<string>? Roles { get; set; }
    public List<string>? Departments { get; set; }
}