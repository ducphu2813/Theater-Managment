using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace UserService.Entity.Model;

public class UserDetail
{
    
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string? FullName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? UserId { get; set; }
}