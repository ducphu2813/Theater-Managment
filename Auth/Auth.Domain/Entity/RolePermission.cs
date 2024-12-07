using Shared.Entity;

namespace Auth.Domain.Entity;

public class RolePermission : BaseEntity
{
    public string? RoleName { get; set; }
    public List<Permission> Permissions { get; set; }
}

public class Permission
{
    public string? Resource { get; set; }
    public List<string>? Actions { get; set; }
}