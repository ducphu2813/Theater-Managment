using Auth.Domain.DTO;
using Auth.Domain.Entity;

namespace Auth.Application.Interfaces;


public interface IUserService
{
    Task<Dictionary<string, object>> GetAllAsync(int page, int limit);
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
    
    //tìm user theo mail
    Task<User> GetUserByEmail(string email);
    
    //hàm xác nhận mail
    Task<User> ConfirmEmailAsync(string email, string token);
    
    //quên mật khẩu
    Task<User> ForgotPasswordAsync(string email);
    
    //đổi mật khẩu của quên mật khẩu
    Task<User> ResetPasswordAsync(ResetPasswordRequest resetPasswordRequest);
}