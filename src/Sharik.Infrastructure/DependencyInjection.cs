using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sharik.Application.Common.Interfaces;
using Sharik.Infrastructure.Auth;
using Sharik.Infrastructure.Data;
using Sharik.Infrastructure.Data.Interceptors;

namespace Sharik.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection _services, IConfiguration _configuration)
    {
        _services.AddSingleton(TimeProvider.System);

        _services.AddDbContext<AppDbContext>((sp, _options) =>
        {
            _options.AddInterceptors(sp.GetRequiredService<ISaveChangesInterceptor>());
            _options.UseNpgsql(_configuration.GetConnectionString("DefaultConnection"));
        }).AddIdentityCore<AppUser>()
        .AddRoles<IdentityRole<Guid>>()
        .AddEntityFrameworkStores<AppDbContext>();

        _services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());

        _services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();

        return _services;
    }
}
