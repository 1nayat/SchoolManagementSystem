using Microsoft.EntityFrameworkCore;
using School.Infrastructure.Persistence;
using School.Infrastructure.Persistence.Seed;

var builder = WebApplication.CreateBuilder(args);

// ==============================
// Services
// ==============================

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// HttpContext (required later for JWT / CurrentUser)
builder.Services.AddHttpContextAccessor();

// DbContext (migrations live in Infrastructure)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sql => sql.MigrationsAssembly("School.Infrastructure")
    )
);

// ==============================
// Seeders (ORDER MATTERS)
// ==============================

builder.Services.AddScoped<RoleSeeder>();
builder.Services.AddScoped<IdentitySeeder>();

// ⚠️ JWT will be added later

// ==============================
// Build
// ==============================

var app = builder.Build();

// ==============================
// Pipeline
// ==============================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Authentication will come later
// app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

// ==============================
// Runtime Seeding (CORRECT ORDER)
// ==============================

using (var scope = app.Services.CreateScope())
{
    var roleSeeder = scope.ServiceProvider.GetRequiredService<RoleSeeder>();
    await roleSeeder.SeedRolesAsync();

    var identitySeeder = scope.ServiceProvider.GetRequiredService<IdentitySeeder>();
    await identitySeeder.SeedSuperAdminAsync();
}

app.Run();
