using AuthService.Entity.Model;
using AuthService.Repository.MongoDBRepo;

namespace AuthService.Repository.Interface;

public interface IRoleRepository : IRepository<RolePermission>
{
    //lấy danh sách quyền theo tên role
    Task<List<RolePermission>> GetRolePermissionByRolename(List<string> rolename);
    
    //lấy danh sách quyền theo tên role
    Task<RolePermission> GetRolePermissionByRolename(string rolename);
}