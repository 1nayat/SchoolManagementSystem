using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using School.Application.Common.Auth;
using School.Infrastructure.Persistence;
using School.Infrastructure.Security;
using System.IO;

namespace School.Infrastructure.Persistence;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        // Build configuration (read appsettings.json)
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlServer(
            configuration.GetConnectionString("DefaultConnection")
        );

        // 👇 Fake current user for design-time
        var designTimeUser = new DesignTimeCurrentUser();

        return new AppDbContext(optionsBuilder.Options, designTimeUser);
    }
}
