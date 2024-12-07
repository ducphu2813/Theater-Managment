namespace Auth.Domain.DTO;

public class ResetPasswordRequest
{
    public string? newPassword { get; set; }
    public string? token { get; set; }
}