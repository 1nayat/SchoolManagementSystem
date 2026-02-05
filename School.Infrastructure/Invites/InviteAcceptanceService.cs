using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using School.Application.Invites.DTOs;
using School.Application.Invites.Interfaces;
using School.Domain.Entities;
using School.Infrastructure.Persistence;
using System.Security.Cryptography;
using System.Text;

public class InviteAcceptanceService : IInviteAcceptanceService
{
    private readonly AppDbContext _db;
    private readonly IPasswordHasher<User> _passwordHasher;

    public InviteAcceptanceService(
        AppDbContext db,
        IPasswordHasher<User> passwordHasher)
    {
        _db = db;
        _passwordHasher = passwordHasher;
    }

    public async Task AcceptAsync(AcceptInviteRequest request)
    {
        var invite = await GetValidInvite(request.Token);

    }

    public async Task SetPasswordAsync(SetPasswordRequest request)
    {
        var invite = await GetValidInvite(request.Token);

        var user = new User
        {
            Email = invite.Email,
            SchoolId = invite.SchoolId,
            IsActive = true
        };

        user.PasswordHash =
            _passwordHasher.HashPassword(user, request.Password);

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        var role = await _db.Roles.FirstAsync(r => r.Name == invite.Role);

        _db.UserRoles.Add(new UserRole
        {
            UserId = user.Id,
            RoleId = role.Id
        });

        var teacher = await _db.Teachers.FindAsync(invite.TeacherId);
        teacher!.UserId = user.Id;
        teacher.IsProfileCompleted = true;

        invite.IsUsed = true;

        await _db.SaveChangesAsync();
    }

    private async Task<UserInvite> GetValidInvite(string rawToken)
    {
        var hashed = HashToken(rawToken);

        var invite = await _db.UserInvites
            .FirstOrDefaultAsync(x => x.Token == hashed);

        if (invite == null)
            throw new InvalidOperationException("Invalid invite");

        if (invite.IsUsed)
            throw new InvalidOperationException("Invite already used");

        if (invite.ExpiresAt < DateTime.UtcNow)
            throw new InvalidOperationException("Invite expired");

        return invite;
    }

    private static string HashToken(string token)
    {
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(token));
        return Convert.ToBase64String(bytes);
    }
}
