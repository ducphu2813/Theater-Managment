using UserService.Entity.Model;
using UserService.Exceptions;
using UserService.Repository.Interface;
using UserService.Service.Interface;

namespace UserService.Service;

public class RolePermissionService : IRolePermissionService
{
    private readonly IRolePermissionRepository _rolePermissionRepository;
    
    public RolePermissionService(IRolePermissionRepository rolePermissionRepository)
    {
        _rolePermissionRepository = rolePermissionRepository;
    }
    
    public async Task<IEnumerable<RolePermission>> GetAllAsync()
    {
        return await _rolePermissionRepository.GetAll();
    }

    public async Task<RolePermission> GetByIdAsync(string id)
    {
        return await _rolePermissionRepository.GetById(id) ?? throw new NotFoundException($"RolePermission with id {id} was not found.");
    }

    public async Task<RolePermission> GetByRoleName(string roleName)
    {
        return await _rolePermissionRepository.GetByRoleName(roleName) ?? throw new NotFoundException($"RolePermission with role name {roleName} was not found.");
    }

    public async Task<RolePermission> AddAsync(RolePermission rolePermission)
    {
        return await _rolePermissionRepository.Add(rolePermission);
    }

    public async Task<RolePermission> UpdateAsync(string id, RolePermission rolePermission)
    {
        RolePermission rolePermissionToUpdate = await _rolePermissionRepository.GetById(id);
        
        if(rolePermissionToUpdate == null)
        {
            throw new NotFoundException($"RolePermission with id {id} was not found.");
        }
        
        return await _rolePermissionRepository.Update(id, rolePermission);
    }

    public async Task<bool> RemoveAsync(string id)
    {
        RolePermission rolePermissionToRemove = await _rolePermissionRepository.GetById(id);
        
        if(rolePermissionToRemove == null)
        {
            throw new NotFoundException($"RolePermission with id {id} was not found.");
        }
        
        return await _rolePermissionRepository.Remove(id);
    }
}