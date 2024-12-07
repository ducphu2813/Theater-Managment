﻿using Auth.Domain.Entity;
using Auth.Domain.Interfaces;
using MongoDB.Driver;
using Shared.Context;
using Shared.Repository;

namespace Auth.Infrastructure.Repository;

public class RoleRepository : MongoDBRepository<RolePermission>, IRoleRepository
{
    public RoleRepository(MongoDBContext context) : base(context, "RolePermission")
    {
    }
    
    //lấy danh sách quyền theo danh sách tên role
    public async Task<List<RolePermission>> GetRolePermissionByRolename(List<string> rolename)
    {
        var filter = Builders<RolePermission>.Filter.In("RoleName", rolename);
        return await _collection.Find(filter).ToListAsync();
    }
    
    //lấy danh sách quyền theo tên role
    public async Task<RolePermission> GetRolePermissionByRolename(string rolename)
    {
        var filter = Builders<RolePermission>.Filter.Eq("RoleName", rolename);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }
    
}