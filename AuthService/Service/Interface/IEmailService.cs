namespace AuthService.Service.Interface;

public interface IEmailService
{
    
    Task SendEmailAsync(string toEmail, string subject, string body);
}