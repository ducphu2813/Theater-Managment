using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PaymentService.Entity.Model;

public class Payment
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    
    public string? UserId { get; set; }
    public string? TicketId { get; set; }
    public string? PaymentMethod { get; set; }
    public string? Status { get; set; }
    public string? Amount { get; set; }
    public DateTime? CreatedAt { get; set; }
    
}