using UserService.Entity.Model;

namespace UserService.Service.Interface;

public interface IRolePermissionService
{
    Task<IEnumerable<RolePermission>> GetAllAsync();
    Task<RolePermission> GetByIdAsync(string id);
    Task<RolePermission> AddAsync(RolePermission rolePermission);
    Task<RolePermission> UpdateAsync(string id, RolePermission rolePermission);
    Task<bool> RemoveAsync(string id);
    
    //hàm tìm bằng role name
    Task<RolePermission> GetByRoleName(string roleName);
}