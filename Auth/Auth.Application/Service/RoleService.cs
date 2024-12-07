using Auth.Application.Interfaces;
using Auth.Domain.Entity;
using Auth.Domain.Exception;
using Auth.Domain.Interfaces;

namespace Auth.Application.Service;

public class RoleService : IRoleService
{
    
    private readonly IRoleRepository _roleRepository;
    
    public RoleService(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }
    
    public async Task<Dictionary<string, object>> GetAllAsync(int page, int limit)
    {
        return await _roleRepository.GetAll(page, limit);
    }
    
    public async Task<RolePermission> GetByIdAsync(string id)
    {
        var role = await _roleRepository.GetById(id);
        
        if (role == null)
        {
            throw new NotFoundException($"Role with id {id} was not found.");
        }
        
        return role;
    }
    
    public async Task<RolePermission> AddAsync(RolePermission role)
    {
        return await _roleRepository.Add(role);
    }
    
    public async Task<RolePermission> UpdateAsync(string id, RolePermission role)
    {
        var existingRole = await _roleRepository.GetById(id);
        
        if (existingRole == null)
        {
            throw new NotFoundException($"Role with id {id} was not found.");
        }
        
        return await _roleRepository.Update(id, role);
    }
    
    public async Task<bool> RemoveAsync(string id)
    {
        var existingRole = await _roleRepository.GetById(id);
        
        if (existingRole == null)
        {
            throw new NotFoundException($"Role with id {id} was not found.");
        }
        
        return await _roleRepository.Remove(id);
    }
    
    //lấy tất cả role permission theo role name
    public async Task<RolePermission> GetRolePermissionsByRoleNameAsync(string roleName)
    {
        return await _roleRepository.GetRolePermissionByRolename(roleName);
    }
}