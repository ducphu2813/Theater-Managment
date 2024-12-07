using Auth.Application.Interfaces;
using Auth.Domain.DTO;
using Auth.Infrastructure.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.API.Controllers;


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
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var result = await _userService.LoginAsync(user.Username, user.Password);
        
        if(result == "unauthorized")
        {
            //trả về mã 401 unauthorized
            return Unauthorized();
        }

        if (result == "notmailverified")
        {
            return BadRequest(new Dictionary<String, String>
            {
                {"status", "fail"},
                {"message", "Please verify your email before login."}
            });
        }
        
        return Ok(new Dictionary<String, String>
        {
            {"status", "success"},
            {"accessToken", result}
        });
    }
}