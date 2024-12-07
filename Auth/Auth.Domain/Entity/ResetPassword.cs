using Shared.Entity;

namespace Auth.Domain.Entity;

public class ResetPassword : BaseEntity
{
    
    public string? UserEmail { get; set; }
    public string? ResetToken { get; set; }
    public DateTime? ExpiryTime { get; set; }
}