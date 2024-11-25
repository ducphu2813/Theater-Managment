using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AuthService.Entity.Model;

public class ResetPassword
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    
    public string? UserEmail { get; set; }
    public string? ResetToken { get; set; }
    public DateTime? ExpiryTime { get; set; }
}