using Microsoft.AspNetCore.Mvc;
using UserService.Entity.Model;
using UserService.Service.Interface;

namespace UserService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserDetailController : ControllerBase
{
    
    private readonly IUserDetailService _userDetailService;
    
    public UserDetailController(IUserDetailService userDetailService)
    {
        _userDetailService = userDetailService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var result = await _userDetailService.GetAllAsync();
        return Ok(result);
    }
    
    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetByIdAsync(string id)
    {
        var result = await _userDetailService.GetByIdAsync(id);
        return Ok(result);
    }
    
    //lấy detail theo userId
    [HttpGet]
    [Route("user/{userId}")]
    public async Task<IActionResult> GetByUserIdAsync(string userId)
    {
        var result = await _userDetailService.GetByUserIdAsync(userId);
        return Ok(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody] UserDetail userDetail)
    {
        
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var result = await _userDetailService.AddAsync(userDetail);
        return Ok(result);
    }
    
    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> UpdateAsync(string id, [FromBody] UserDetail userDetail)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var result = await _userDetailService.UpdateAsync(id, userDetail);
        return Ok(result);
    }
    
    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> RemoveAsync(string id)
    {
        var result = await _userDetailService.RemoveAsync(id);
        return Ok(result);
    }
}