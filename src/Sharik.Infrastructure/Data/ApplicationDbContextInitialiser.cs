using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sharik.Domain.User;
using Sharik.Domain.User.Enums;
using Sharik.Infrastructure.Auth;
using Sharik.Infrastructure.Data;

namespace Sharik.Infrastructure.Data
{
    public sealed class ApplicationDbContextInitialiser(AppDbContext context, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, ILogger<ApplicationDbContextInitialiser> logger)
    {
        private readonly AppDbContext _context = context;
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly RoleManager<AppRole> _roleManager = roleManager;
        private readonly ILogger<ApplicationDbContextInitialiser> _logger = logger;

        public async Task InitialiseAsync()
        {
            try
            {
                _context.Database.EnsureCreated();
            }
            catch (Exception ex)

            {
                _logger.LogError(ex, "An error occurred while initialising the database.");
                throw;
            }
        }
        public async Task SeedAsync()
        {
            try
            {
                await TrySeedAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }
        public async Task TrySeedAsync()
        {
            var superAdmin = nameof(Role.SuperAdmin);

            if (_roleManager.Roles.All(r => r.Name != superAdmin))
            {
                var role = AppRole.Create(superAdmin);
                await _roleManager.CreateAsync(role);
            }

            var superAdminResult = AppUser.Create("SuperAdmin", "admin@example.com");

            var admin = superAdminResult.Value;

            if (_userManager.Users.All(u => u.Email != admin.Email))
            {
                await _userManager.CreateAsync(admin, "Admin123$");
                await _userManager.AddToRoleAsync(admin, superAdmin!);
            }

            var userRole = nameof(Role.User);

            if (_roleManager.Roles.All(r => r.Name != userRole))
            {
                var role = AppRole.Create(userRole);
                await _roleManager.CreateAsync(role);
            }

            var adminRole = nameof(Role.Admin);

            if (_roleManager.Roles.All(r => r.Name != adminRole))
            {
                var role = AppRole.Create(adminRole);
                await _roleManager.CreateAsync(role);
            }
        }
    }
}
public static class InitialiserExtensions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();

        await initialiser.InitialiseAsync();

        await initialiser.SeedAsync();
    }
}
