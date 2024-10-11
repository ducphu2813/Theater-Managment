using AuthService.Entity.Model;

namespace AuthService.Service.Interface;

public interface IRoleService
{
    Task<IEnumerable<RolePermission>> GetAllAsync();
    Task<RolePermission> GetByIdAsync(string id);
    Task<RolePermission> AddAsync(RolePermission role);
    Task<RolePermission> UpdateAsync(string id, RolePermission role);
    Task<bool> RemoveAsync(string id);
    
    //lấy tất cả role permission theo role name
    Task<RolePermission> GetRolePermissionsByRoleNameAsync(string roleName);
}