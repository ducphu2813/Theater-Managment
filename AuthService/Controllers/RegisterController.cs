using AuthService.Entity.Model;
using AuthService.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class RegisterController : ControllerBase
{
    private readonly IUserService _userService;
    
    public RegisterController(IUserService userService)
    {
        _userService = userService;
    }
    
    
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
    {
        
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        //lưu thông tin user vào database
        //catch exception nếu user đã tồn tại
        User result;
        try
        {
            result = await _userService.RegisterAsync(registerRequest);
        }
        catch (Exception e)
        {
            return BadRequest(new Dictionary<String, String>
            {
                {"status", "fail"},
                {"message", e.Message}
            });
        }
        
        //login sau khi register
        var loginResult = await _userService.LoginAsync(result.Username, result.Password);
        
        return Ok(new Dictionary<String, String>
        {
            {"status", "success"},
            {"accessToken", loginResult}
        });
    }
}