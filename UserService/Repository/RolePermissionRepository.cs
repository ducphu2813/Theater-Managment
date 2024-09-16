using MongoDB.Driver;
using UserService.Context;
using UserService.Entity.Model;
using UserService.Repository.Interface;
using UserService.Repository.MongoDBRepo;

namespace UserService.Repository;

public class RolePermissionRepository : MongoDBRepository<RolePermission>, IRolePermissionRepository
{
    public RolePermissionRepository(MongoDBContext context) : base(context, "RolePermission")
    {
    }
    
    //hàm tìm bằng role name
    public async Task<RolePermission> GetByRoleName(string roleName)
    {
        var filter = Builders<RolePermission>.Filter.Eq("RoleName", roleName);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }
    
}