using UserService.Entity.Model;

namespace UserService.Service.Interface;

public interface IUserService
{
    Task<IEnumerable<User>> GetAllAsync();
    Task<User> GetByIdAsync(string id);
    Task<User> AddAsync(User user);
    Task<User> UpdateAsync(string id, User user);
    Task<bool> RemoveAsync(string id);
    
}