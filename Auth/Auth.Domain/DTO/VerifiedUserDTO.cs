namespace Auth.Domain.DTO;

public class VerifiedUserDTO
{
    public string? Id { get; set; }

    public string? Username { get; set; }
    public string? Email { get; set; }
    public List<string>? Roles { get; set; }
    public List<string>? Departments { get; set; }
}