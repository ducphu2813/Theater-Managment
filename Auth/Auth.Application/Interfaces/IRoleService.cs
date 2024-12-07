using Auth.Domain.Entity;

namespace Auth.Application.Interfaces;

public interface IRoleService
{
    Task<Dictionary<string, object>> GetAllAsync(int page, int limit);
    Task<RolePermission> GetByIdAsync(string id);
    Task<RolePermission> AddAsync(RolePermission role);
    Task<RolePermission> UpdateAsync(string id, RolePermission role);
    Task<bool> RemoveAsync(string id);
    
    //lấy tất cả role permission theo role name
    Task<RolePermission> GetRolePermissionsByRoleNameAsync(string roleName);
}