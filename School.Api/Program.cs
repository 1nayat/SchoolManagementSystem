using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using School.Application.Common.Auth;
using School.Application.Common.Email;
using School.Application.Invites.Interfaces;
using School.Application.Teachers.Interfaces;
using School.Domain.Entities;
using School.Infrastructure.Auth;
using School.Infrastructure.Persistence;
using School.Infrastructure.Persistence.Seed;
using School.Infrastructure.Security;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "School Management API",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter: Bearer {your JWT token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


builder.Services.AddHttpContextAccessor();


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sql => sql.MigrationsAssembly("School.Infrastructure")
    )
);


builder.Services.Configure<JwtOptions>(
    builder.Configuration.GetSection("Jwt"));


builder.Services.AddScoped<ITokenService, JwtTokenService>();

builder.Services.AddScoped<CurrentUser>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<ICurrentUser, CurrentUser>();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwt = builder.Configuration.GetSection("Jwt");

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwt["Issuer"],
            ValidAudience = jwt["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwt["Key"]!)
            ),

            NameClaimType = CustomClaims.UserId,
            RoleClaimType = ClaimTypes.Role
        };
    });


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Policies.RequireSuperAdmin, policy =>
        policy.RequireRole(Roles.SuperAdmin));

    options.AddPolicy(Policies.RequireSchoolAdmin, policy =>
        policy.RequireRole(Roles.SchoolAdmin));

    options.AddPolicy(Policies.RequireOnboardedSchoolAdmin, policy =>
        policy.RequireAssertion(context =>
        {
            var isSchoolAdmin = context.User.IsInRole(Roles.SchoolAdmin);
            var hasSchoolId = context.User.HasClaim(
                c => c.Type == CustomClaims.SchoolId
            );

            return isSchoolAdmin && hasSchoolId;
        }));
});


builder.Services.AddScoped<SchoolSeeder>();
builder.Services.AddScoped<RoleSeeder>();
builder.Services.AddScoped<IdentitySeeder>();
builder.Services.AddScoped<IEmailService, SmtpEmailService>();
builder.Services.AddScoped<ITeacherInviteService, TeacherInviteService>();
builder.Services.AddScoped<ITeacherService, TeacherService>();
builder.Services.AddScoped<IInviteAcceptanceService, InviteAcceptanceService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


using (var scope = app.Services.CreateScope())
{
    var schoolSeeder = scope.ServiceProvider.GetRequiredService<SchoolSeeder>();
    var roleSeeder = scope.ServiceProvider.GetRequiredService<RoleSeeder>();
    var identitySeeder = scope.ServiceProvider.GetRequiredService<IdentitySeeder>();

    await schoolSeeder.SeedAsync();
    await roleSeeder.SeedRolesAsync();
    await identitySeeder.SeedAsync();
}

app.Run();
