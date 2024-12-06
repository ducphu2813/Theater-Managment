using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ReservationService.Core.Entity.Model;

public class Food
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    
    public string? Name { get; set; }
    public string? FoodType { get; set; }
    public string? Description { get; set; }
    public int? Amount { get; set; }
    public string? Image { get; set; }
}