using Shared.Entity;

namespace Auth.Domain.Entity;

public class User : BaseEntity
{
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? Email { get; set; }
    public string? isEmailVerified { get; set; }
    public string? confirmMailToken { get; set; }
    public List<string>? Roles { get; set; }
    public List<string>? Departments { get; set; }
}