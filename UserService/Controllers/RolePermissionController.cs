using Microsoft.AspNetCore.Mvc;
using UserService.Entity.Model;
using UserService.Service.Interface;

namespace UserService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RolePermissionController : ControllerBase
{
    private readonly IRolePermissionService _rolePermissionService;
    
    public RolePermissionController(IRolePermissionService rolePermissionService)
    {
        _rolePermissionService = rolePermissionService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var result = await _rolePermissionService.GetAllAsync();
        return Ok(result);
    }
    
    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetByIdAsync(string id)
    {
        var result = await _rolePermissionService.GetByIdAsync(id);
        return Ok(result);
    }
    
    [HttpGet]
    [Route("role/{roleName}")]
    public async Task<IActionResult> GetByRoleName(string roleName)
    {
        var result = await _rolePermissionService.GetByRoleName(roleName);
        return Ok(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody] RolePermission rolePermission)
    {
        
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var result = await _rolePermissionService.AddAsync(rolePermission);
        return Ok(result);
    }
    
    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> UpdateAsync(string id, [FromBody] RolePermission rolePermission)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var result = await _rolePermissionService.UpdateAsync(id, rolePermission);
        return Ok(result);
    }
    
    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> RemoveAsync(string id)
    {
        var result = await _rolePermissionService.RemoveAsync(id);
        return Ok(result);
    }
    
}