using AuthService.Context;
using AuthService.Entity.Model;
using AuthService.Repository.Interface;
using AuthService.Repository.MongoDBRepo;
using MongoDB.Driver;

namespace AuthService.Repository;

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
    
}