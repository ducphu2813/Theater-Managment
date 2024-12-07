using Auth.Application.Interfaces;
using Auth.Domain.DTO;
using Auth.Domain.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Auth.API.Controllers;


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
        
        //tạo verified user dto
        var verifiedUser = new VerifiedUserDTO
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Roles = user.Roles,
        };
        
        return Ok(verifiedUser);
    }
    
}