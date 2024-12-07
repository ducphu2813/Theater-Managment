using Auth.Application.Interfaces;
using Auth.Domain.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Auth.API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class MailController : ControllerBase
{
    
    private readonly IEmailService _emailService;
    private readonly IUserService _userService;
    
    public MailController(IEmailService emailService
        , IUserService userService)
    {
        _emailService = emailService;
        _userService = userService;
    }
    
    //test gửi mail
    [HttpPost]
    [Route("send")]
    public async Task<IActionResult> SendMailAsync([FromBody] MailRequest mailRequest)
    {
        await _emailService.SendEmailAsync(mailRequest.ToEmail, mailRequest.Subject, mailRequest.Body);
        return Ok();
    }
    
    //hàm xác nhận email
    [HttpGet]
    [Route("confirm")]
    public async Task<IActionResult> ConfirmEmailAsync()
    {
        //lấy email từ param
        string email = Request.Query["email"];
        string token = Request.Query["token"];
        if(string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
        {
            return BadRequest();
        }
        try
        {
            //xác nhận mail
            var result = await _userService.ConfirmEmailAsync(email, token);
            //nếu xác nhận thành công
            return Ok(new Dictionary<string, string>
            {
                {"status", "success"},
                {"message", "Confirm email success, please login to continue."}
            });
        }
        catch (Exception e)
        {
            return BadRequest();
        }
        
    }
    
}