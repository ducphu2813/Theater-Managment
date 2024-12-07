using Auth.Domain.Entity;
using Auth.Domain.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using Shared.Context;
using Shared.Repository;

namespace Auth.Infrastructure.Repository;

public class UserRepository : MongoDBRepository<User>, IUserRepository
{
    public UserRepository(MongoDBContext context) : base(context, "User")
    {
    }
    
    //hàm login check username và password
    public async Task<User> LoginAsync(string username, string password)
    {
        var filter = Builders<User>.Filter.Eq("Username", username) & Builders<User>.Filter.Eq("Password", password);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }
    
    //lấy user theo username
    public async Task<User> GetUserByUsername(string username)
    {
        var filter = Builders<User>.Filter.Eq("Username", username);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }
    
    //lấy user theo email
    public async Task<User> GetUserByEmail(string email)
    {
        var filter = Builders<User>.Filter.Eq("Email", email);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }
    
    //lấy tất cả user theo username và role
    public async Task<Dictionary<string, object>> GetAllAdvance(int page, int limit, string username, List<string> roles)
    {
        //lấy count record
        var totalRecords = await _collection.CountDocumentsAsync(Builders<User>.Filter.Empty);
        
        //tạo filter
        var filters = new List<FilterDefinition<User>>();
        
        //kiểm tra param username
        if (!string.IsNullOrEmpty(username))
        {
            filters.Add(Builders<User>.Filter.Regex(u => u.Username, new BsonRegularExpression(username, "i"))); // i để không phân biệt hoa thường
        }
        
        //kiểm tra param roles
        if (roles != null && roles.Count > 0)
        {
            filters.Add(Builders<User>.Filter.AnyIn(u => u.Roles, roles));
        }
        
        //kết hợp các filter
        var combinedFilter = filters.Count > 0 ? Builders<User>.Filter.And(filters) : Builders<User>.Filter.Empty;
        
        //lấy data theo page và limit
        var records = await _collection
            .Find(combinedFilter) //sử dụng filter
            .Skip((page - 1) * limit)
            .Limit(limit)
            .ToListAsync();
        
        //gộp lại thành 1 dictionary
        return new Dictionary<string, object>
        {
            { "totalRecords", totalRecords },
            { "records", records },
            { "limit", limit },
            { "page", page }
        };
    }
    
}