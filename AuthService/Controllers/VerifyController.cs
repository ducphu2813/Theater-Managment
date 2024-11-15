using AuthService.Entity.Model;
using AuthService.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VerifyController : ControllerBase
{
    
    private readonly IUserService _userService;
    
    public VerifyController(IUserService userService)
    {
        _userService = userService;
    }
    
    public async Task<IActionResult> Verify()
    {
        var userId = Request.Query["userId"];
        Console.WriteLine("user id : ",userId);
        
        User user = await _userService.GetByIdAsync(userId);
        
        if (user == null)
        {
            return BadRequest("User not found");
        }
        
        return Ok(user);
    }
    
}