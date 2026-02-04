using School.Domain.Common;

public class UserInvite : TenantEntity
{
    public Guid Id { get; set; }

    public string Email { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string Token { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }

    public bool IsUsed { get; set; }

    public Guid? TeacherId { get; set; }

    public DateTime CreatedAt { get; set; }
}
