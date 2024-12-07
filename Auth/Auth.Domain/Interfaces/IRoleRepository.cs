using Auth.Domain.Entity;
using Shared.Interfaces;

namespace Auth.Domain.Interfaces;

public interface IRoleRepository : IRepository<RolePermission>
{
    //lấy danh sách quyền theo tên role
    Task<List<RolePermission>> GetRolePermissionByRolename(List<string> rolename);
    
    //lấy danh sách quyền theo tên role
    Task<RolePermission> GetRolePermissionByRolename(string rolename);
}