using UserService.Entity.Model;
using UserService.Repository.MongoDBRepo;

namespace UserService.Repository.Interface;

public interface IRolePermissionRepository : IRepository<RolePermission>
{
    //hàm tìm bằng role name
    Task<RolePermission> GetByRoleName(string roleName);
}