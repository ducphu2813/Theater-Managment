using MongoDB.Bson;
using MongoDB.Driver;
using Shared.Settings;

namespace Shared.Context;

public class MongoDBContext
{
    private readonly IMongoDatabase _database;
    private readonly IMongoClient _mongoClient;
    
    public MongoDBContext(MongoDBSettings settings)
    {
        var client = new MongoClient(settings.ConnectionString);
        _database = client.GetDatabase(settings.DatabaseName);
        _mongoClient = new MongoClient(settings.ConnectionString);
        CreateTTLIndexForTicket();
    }
    
    public IMongoCollection<TEntity> GetCollection<TEntity>(string name)
    {
        return _database.GetCollection<TEntity>(name);
    }
    
    //tạo 1 ttl index tự động xóa record trong database Reservation bảng Ticket dựa trên expiry time
    public void CreateTTLIndexForTicket()
    {
        // "Reservation"
        var reservationDatabase = _mongoClient.GetDatabase("Reservation");

        //collection "Ticket"
        var ticketCollection = reservationDatabase.GetCollection<BsonDocument>("Ticket");
        
        //tạo index trên cột ExpiryTime
        var indexKeys = Builders<BsonDocument>.IndexKeys.Ascending("ExpiryTime");
        var indexOptions = new CreateIndexOptions { ExpireAfter = TimeSpan.Zero };

        var indexModel = new CreateIndexModel<BsonDocument>(indexKeys, indexOptions);

        // tạo index
        ticketCollection.Indexes.CreateOne(indexModel);

    }
    
}