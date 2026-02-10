using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Sharik.Application.Common.Interfaces;
using Sharik.Domain.User;
using Sharik.Infrastructure.Auth;
using Sharik.Infrastructure.Data;
using Sharik.Infrastructure.Data.Interceptors;
using System.Text;

namespace Sharik.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services , IConfiguration configuration)
    {
        services.AddSingleton(TimeProvider.System);
        services.AddDatabaseContext(configuration)
                .AddAuthenticationService(configuration)
                .AddAuthorizationService()
                .addCaching(configuration);

        return services;
    }

    public static IServiceCollection AddAuthenticationService(this IServiceCollection services , IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {

            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

        }).AddJwtBearer(options =>
        {
            var jwtSettings = configuration.GetSection("JwtSettings");

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true ,
                ValidateAudience = true ,
                ValidateLifetime = true ,
                ValidateIssuerSigningKey = true ,
                ClockSkew = TimeSpan.Zero ,
                ValidAudience = jwtSettings["Audience"] ,
                ValidIssuer = jwtSettings["Issuer"] ,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]!))
            };
        });

        services.AddScoped<ITokenProvider , TokenProvider>();

        services.AddScoped<IUserService , UserService>();

        services.AddScoped<ApplicationDbContextInitialiser>();

        return services;
    }

    public static IServiceCollection AddAuthorizationService(this IServiceCollection services)
    {
        services.AddAuthorization();

        return services;
    }


    public static IServiceCollection AddDatabaseContext(this IServiceCollection services , IConfiguration configuration)
    {
        services.AddScoped<ISaveChangesInterceptor , AuditableEntityInterceptor>();

        services.AddScoped<ISaveChangesInterceptor , SoftDeleteInterceptor>();
        services.AddDbContext<AppDbContext>((sp , options) =>
        {
            var interceptors = sp.GetServices<ISaveChangesInterceptor>();

            options.AddInterceptors(interceptors);
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));

        }).AddIdentityCore<AppUser>()
        .AddRoles<AppRole>()
        .AddEntityFrameworkStores<AppDbContext>();

        services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());

        return services;
    }

    public static IServiceCollection addCaching(this IServiceCollection services , IConfiguration configuration)
    {


        services.AddDistributedPostgresCache(option =>
        {
            option.ConnectionString = configuration.GetConnectionString("DefaultConnection");
            option.SchemaName = "cache";
            option.TableName = "cache_entries";
            option.CreateIfNotExists = true;
        });

        services.AddHybridCache(option => {

            option.DefaultEntryOptions = new HybridCacheEntryOptions
            {
                Expiration = TimeSpan.FromMinutes(1),
                LocalCacheExpiration = TimeSpan.FromSeconds(10)
            };
        });

        return services;
    }


}
