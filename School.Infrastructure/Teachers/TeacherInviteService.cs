using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using School.Application.Common.Auth;
using School.Application.Common.Email;
using School.Application.Invites;
using School.Application.Teachers.DTOs;
using School.Application.Teachers.Interfaces;
using School.Domain.Entities;
using School.Infrastructure.Persistence;
using System.Security.Cryptography;
using System.Text;

public class TeacherInviteService : ITeacherInviteService
{
    private readonly AppDbContext _db;
    private readonly IEmailService _emailService;
    private readonly ICurrentUser _currentUser;

    public TeacherInviteService(
        AppDbContext db,
        IEmailService emailService,
        ICurrentUser currentUser)
    {
        _db = db;
        _emailService = emailService;
        _currentUser = currentUser;
    }

    public async Task<string?> InviteAsync(InviteTeacherRequest request)
    {
        var schoolId = _currentUser.SchoolId
            ?? throw new UnauthorizedAccessException();

        var exists = await _db.Users.AnyAsync(u => u.Email == request.Email);
        if (exists)
            throw new InvalidOperationException("User already exists");

        var teacher = new Teacher
        {
            SchoolId = schoolId,
            EmployeeCode = request.EmployeeCode,
            FirstName = request.FirstName,
            LastName = request.LastName,
            TeacherType = request.TeacherType,
            IsProfileCompleted = false
        };

        _db.Teachers.Add(teacher);

        var rawToken = WebEncoders.Base64UrlEncode(
            RandomNumberGenerator.GetBytes(32)
        );

        var invite = new UserInvite
        {
            SchoolId = schoolId,
            Email = request.Email,
            Role = "Teacher",
            Token = rawToken,          
            ExpiresAt = DateTime.UtcNow.AddHours(48),
            IsUsed = false,
            TeacherId = teacher.Id,
            CreatedAt = DateTime.UtcNow
        };

        _db.UserInvites.Add(invite);
        await _db.SaveChangesAsync();

        var inviteLink =
            $"https://localhost:4200/invite/accept?token={rawToken}";

        var (subject, body) =
            InviteEmailBuilder.BuildTeacherInvite("Your School", inviteLink);

        await _emailService.SendAsync(
            request.Email,
            subject,
            body
        );

        return rawToken; 
    }

    private static string HashToken(string token)
    {
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(token));
        return Convert.ToBase64String(bytes);
    }
}
