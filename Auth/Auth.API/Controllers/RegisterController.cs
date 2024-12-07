using Auth.Application.Interfaces;
using Auth.Domain.DTO;
using Auth.Domain.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.API.Controllers;


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
            await _emailService.SendEmailAsync(result.Email, "Confirm Email", "Click here to confirm email: http://localhost:5173/validaccount?email=" + result.Email + "&token=" + result.confirmMailToken);
        
            return Ok(new Dictionary<String, String>
            {
                {"status", "success"},
                {"message", "Please check your email to confirm your account."}
            });
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(new Dictionary<String, String>
            {
                {"status", "fail"},
                {"message", e.Message}
            });
        }
    }
}