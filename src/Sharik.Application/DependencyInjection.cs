using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Sharik.Application.Common.Behaviors;
using Sharik.Application.Common.Interfaces;
using Sharik.Application.Featuers.Exchanges.Validation;
using System.Reflection;

namespace Sharik.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            cfg.AddOpenBehavior(typeof(CachingBehavior<,>));


        });
        services.AddScoped<IExchangeBusinessValidator , ExchangeBusinessValidator>();

        return services;
    }   
}
