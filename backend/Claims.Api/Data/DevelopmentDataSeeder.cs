using Claims.Api.Enums;
using Claims.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Claims.Api.Data;

public static class DevelopmentDataSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();

        var context = scope.ServiceProvider
            .GetRequiredService<AppDbContext>();

        await context.Database.MigrateAsync();

        if (await context.Users.AnyAsync())
        {
            return;
        }

        var users = new List<AppUser>
        {
            new()
            {
                FullName = "Alex Morgan",
                Email = "alex.claimant@example.test",
                Role = UserRole.Claimant
            },
            new()
            {
                FullName = "Sarah Johnson",
                Email = "sarah.officer@example.test",
                Role = UserRole.ClaimsOfficer
            },
            new()
            {
                FullName = "David Wilson",
                Email = "david.manager@example.test",
                Role = UserRole.Manager
            }
        };

        context.Users.AddRange(users);

        await context.SaveChangesAsync();
    }
}