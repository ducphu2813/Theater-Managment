using AuthService.Entity.Model;
using AuthService.Repository.MongoDBRepo;

namespace AuthService.Repository.Interface;

public interface IUserRepository : IRepository<User>
{
    //hàm login
    Task<User> LoginAsync(string username, string password);
    
    //lấy user theo username
    Task<User> GetUserByUsername(string username);
    
    //lấy tất cả user theo username và role
    Task<Dictionary<string, object>> GetAllAdvance(int page, int limit, string username, List<string> roles);
}