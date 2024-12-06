using MongoDB.Driver;
using ReservationService.Core.Entity;
using ReservationService.Core.Entity.Model;

namespace ReservationService.Infrastructure.Persistence.Context;

public class MongoDBContext
{
    private readonly IMongoDatabase _database;
    
    public MongoDBContext(MongoDBSettings settings)
    {
        var client = new MongoClient(settings.ConnectionString);
        _database = client.GetDatabase(settings.DatabaseName);
        
        CreateTTLIndexForTicket();
    }
    
    public IMongoCollection<TEntity> GetCollection<TEntity>(string name)
    {
        return _database.GetCollection<TEntity>(name);
    }
    
    //tạo 1 ttl index tự động xóa ticket dựa trên expiry time
    private void CreateTTLIndexForTicket()
    {
        var ticketCollection = _database.GetCollection<Ticket>("Ticket");
    
        var indexKeys = Builders<Ticket>.IndexKeys.Ascending(ticket => ticket.ExpiryTime);
        var indexOptions = new CreateIndexOptions { ExpireAfter = TimeSpan.Zero };

        var indexModel = new CreateIndexModel<Ticket>(indexKeys, indexOptions);
        ticketCollection.Indexes.CreateOne(indexModel);
    }
    
}