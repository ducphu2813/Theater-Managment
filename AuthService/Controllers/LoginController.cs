using AuthService.Entity.Model;
using AuthService.Service.Interface;
using AuthService.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class LoginController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly TokenProvider _tokenProvider;
    
    public LoginController(IUserService userService)
    {
        _userService = userService;
    }
    
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginRequest user)
    {
        var result = await _userService.LoginAsync(user.Username, user.Password);
        
        if(result == "unauthorized")
        {
            //trả về mã 401 unauthorized
            return Unauthorized();
        }
        
        return Ok(result);
    }
}