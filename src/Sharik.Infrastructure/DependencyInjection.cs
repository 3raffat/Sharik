using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sharik.Infrastructure.Auth;
using Sharik.Infrastructure.Data;

namespace Sharik.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection _services, IConfiguration _configuration)
    {
        _services.AddDbContext<AppDbContext>(_options =>
        {
            _options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
        }).AddIdentityCore<AppUser>()
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<AppDbContext>();


        return _services;
    }
}
