using AuthService.Entity.Model;
using AuthService.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    
    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var result = await _userService.GetAllAsync();
        return Ok(result);
    }
    
    //lấy tất cả user theo username và role
    [HttpGet]
    [Route("getAll")]
    public async Task<IActionResult> GetAllAdvanceAsync()
    {
        //lấy ra các param phân trang
        var page = int.Parse(Request.Query["page"]);
        var limit = int.Parse(Request.Query["limit"]);
        
        //lấy ra các param tìm kiếm nâng cao
        var username = Request.Query["username"];
        //lấy roles từ param, lưu thành 1 mảng string
        var roles = Request.Query["roles"]
            .ToString()          //convert thành string
            .Split(",", StringSplitOptions.RemoveEmptyEntries)  //tách theo dấu ,
            .ToList();  //thành mảng
        var result = await _userService.GetAllAdvance(page, limit, username, roles);
        return Ok(result);
    }
    
    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetByIdAsync(string id)
    {
        var result = await _userService.GetByIdAsync(id);
        return Ok(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody] User user)
    {
        
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var result = await _userService.AddAsync(user);
        return Ok(result);
    }
    
    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> UpdateAsync(string id, [FromBody] User user)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var result = await _userService.UpdateAsync(id, user);
        return Ok(result);
    }
    
    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> RemoveAsync(string id)
    {
        var result = await _userService.RemoveAsync(id);
        return Ok(result);
    }
    
    //hàm lấy role theo username
    [HttpGet]
    [Route("role-permission/{username}")]
    public async Task<IActionResult> GetRolePermissionsByUsernameAsync(string username)
    {
        var result = await _userService.GetRolePermissionsByUsernameAsync(username);
        return Ok(result);
    }
}