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
    private readonly IEmailService _emailService;
    
    public RegisterController(IUserService userService
    , IEmailService emailService)
    {
        _userService = userService;
        _emailService = emailService;
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
        //catch các exception
        User result;
        try
        {
            result = await _userService.RegisterAsync(registerRequest);
            
            //gửi mail xác nhận
            await _emailService.SendEmailAsync(result.Email, "Confirm Email", "Click here to confirm email: http://localhost:5006/auth/api/Mail/confirm?email=" + result.Email + "&token=" + result.confirmMailToken);
        
            return Ok(new Dictionary<String, String>
            {
                {"status", "success"},
                {"message", "Please check your email to confirm your account."}
            });
        }
        catch (Exception e)
        {
            return BadRequest(new Dictionary<String, String>
            {
                {"status", "fail"},
                {"message", e.Message}
            });
        }
    }
}