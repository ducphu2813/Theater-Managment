namespace Auth.Domain.DTO;

public class RegisterRequest
{
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? Email { get; set; }
    public List<string>? Roles { get; set; }
    public List<string>? Departments { get; set; }
}