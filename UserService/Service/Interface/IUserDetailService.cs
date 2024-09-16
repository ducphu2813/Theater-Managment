using UserService.Entity.Model;

namespace UserService.Service.Interface;

public interface IUserDetailService
{
    Task<IEnumerable<UserDetail>> GetAllAsync();
    Task<UserDetail> GetByIdAsync(string id);
    Task<UserDetail> AddAsync(UserDetail userDetail);
    Task<UserDetail> UpdateAsync(string id, UserDetail userDetail);
    Task<bool> RemoveAsync(string id);
    
    //hàm lấy user detail theo user id
    Task<UserDetail> GetByUserIdAsync(string userId);
    
}