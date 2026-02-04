using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using School.Application.Common.Auth;
using School.Domain.Entities;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace School.Infrastructure.Auth;

public class JwtTokenService : ITokenService
{
    private readonly JwtOptions _options;

    public JwtTokenService(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }

    public string GenerateToken(User user)
    {
        if (user.UserRoles == null || !user.UserRoles.Any())
            throw new InvalidOperationException("User has no roles assigned.");

        var roleNames = user.UserRoles
            .Select(ur => ur.Role.Name)
            .Distinct()
            .ToList();

        var isSuperAdmin = roleNames.Contains(Roles.SuperAdmin);
        var isSchoolAdmin = roleNames.Contains(Roles.SchoolAdmin);

        // 🔒 Only non-admin tenant users MUST have SchoolId
        var requiresSchool =
            !isSuperAdmin &&
            !isSchoolAdmin;

        if (requiresSchool && user.SchoolId == null)
        {
            throw new InvalidOperationException(
                "User must belong to a school."
            );
        }

        var claims = new List<Claim>
    {
        new Claim(CustomClaims.UserId, user.Id.ToString()),
        new Claim(ClaimTypes.Email, user.Email)
    };

        foreach (var role in roleNames)
        {
            claims.Add(new Claim(CustomClaims.Role, role));
        }

        if (user.SchoolId.HasValue)
        {
            claims.Add(new Claim(
                CustomClaims.SchoolId,
                user.SchoolId.Value.ToString()
            ));
        }

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_options.Key)
        );

        var creds = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256
        );

        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_options.ExpiresInMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
