using MongoDB.Bson;
using MongoDB.Driver;
using PaymentService.Context;

namespace PaymentService.Repository.MongoDBRepo;

public class MongoDBRepository <TEntity> : IRepository<TEntity> where TEntity : class
{
    protected readonly IMongoCollection<TEntity> _collection;

    public MongoDBRepository(MongoDBContext context, string collectionName)
    {
        _collection = context.GetCollection<TEntity>(collectionName);
    }
    
    //tìm tất cả dữ liệu
    public virtual async Task<IEnumerable<TEntity>> GetAll()
    {
        return await _collection.Find(Builders<TEntity>.Filter.Empty).ToListAsync();
    }

    //tìm theo id
    public virtual async Task<TEntity> GetById(string id)
    {
        return await _collection.Find(FilterId(id)).SingleOrDefaultAsync();
    }
    
    //tìm theo nhiều id
    public virtual async Task<List<TEntity>> GetAllByIds(List<string> ids)
    {
        var filter = Builders<TEntity>.Filter.In("Id", ids.Select(ObjectId.Parse));
        return await _collection.Find(filter).ToListAsync();
    }

    //thêm
    public virtual async Task<TEntity> Add(TEntity obj)
    {
        await _collection.InsertOneAsync(obj);
        return obj;
    }

    //cập nhật
    public virtual async Task<TEntity> Update(string id, TEntity obj)
    {
        await _collection.ReplaceOneAsync(FilterId(id), obj);
        return obj;
    }

    //xóa
    public virtual async Task<bool> Remove(string id)
    {
        var result = await _collection.DeleteOneAsync(FilterId(id));
        return result.DeletedCount > 0;
    }
    
    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
    
    //hàm filter hỗ trợ tìm kiếm theo id
    private static FilterDefinition<TEntity> FilterId(string key)
    {
        return Builders<TEntity>.Filter.Eq("Id", key);
    }
}