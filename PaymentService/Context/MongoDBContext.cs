using MongoDB.Driver;
using PaymentService.Entity;

namespace PaymentService.Context;

public class MongoDBContext
{
    private readonly IMongoDatabase _database;
    
    public MongoDBContext(MongoDBSettings settings)
    {
        var client = new MongoClient(settings.ConnectionString);
        _database = client.GetDatabase(settings.DatabaseName);
    }
    
    public IMongoCollection<TEntity> GetCollection<TEntity>(string name)
    {
        return _database.GetCollection<TEntity>(name);
    }
    
}