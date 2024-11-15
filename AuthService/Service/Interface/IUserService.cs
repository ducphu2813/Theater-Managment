using AuthService.Entity.Model;

namespace AuthService.Service.Interface;

public interface IUserService
{
    Task<IEnumerable<User>> GetAllAsync();
    Task<User> GetByIdAsync(string id);
    Task<User> AddAsync(User user);
    Task<User> UpdateAsync(string id, User user);
    Task<bool> RemoveAsync(string id);
    
    //lấy user theo username
    Task<User> GetUserByUsername(string username);
    
    //hàm login
    Task<string> LoginAsync(string username, string password);
    
    //hàm register
    Task<User> RegisterAsync(RegisterRequest registerRequest);
    
    //lấy tất cả role permission của user theo username
    Task<List<RolePermission>> GetRolePermissionsByUsernameAsync(string username);
    
    //lấy tất cả user theo username và role
    Task<Dictionary<string, object>> GetAllAdvance(int page, int limit, string username, List<string> roles);
}