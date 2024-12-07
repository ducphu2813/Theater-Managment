using Auth.Domain.Entity;
using Shared.Interfaces;

namespace Auth.Domain.Interfaces;

public interface IUserRepository : IRepository<User>
{
    //hàm login
    Task<User> LoginAsync(string username, string password);
    
    //lấy user theo username
    Task<User> GetUserByUsername(string username);
    
    //lấy user theo email
    Task<User> GetUserByEmail(string email);
    
    //lấy tất cả user theo username và role
    Task<Dictionary<string, object>> GetAllAdvance(int page, int limit, string username, List<string> roles);
    
}