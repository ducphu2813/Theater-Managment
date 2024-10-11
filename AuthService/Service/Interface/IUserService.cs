using AuthService.Entity.Model;

namespace AuthService.Service.Interface;

public interface IUserService
{
    Task<IEnumerable<User>> GetAllAsync();
    Task<User> GetByIdAsync(string id);
    Task<User> AddAsync(User user);
    Task<User> UpdateAsync(string id, User user);
    Task<bool> RemoveAsync(string id);
    
    //hàm login
    Task<string> LoginAsync(string username, string password);
    
    //lấy tất cả role permission của user theo username
    Task<List<RolePermission>> GetRolePermissionsByUsernameAsync(string username);
}