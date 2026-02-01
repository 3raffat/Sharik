using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Sharik.Application.Common.Behaviors;
using System.Reflection;

namespace Sharik.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection _services)
    {
        _services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        _services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));

        });

        return _services;
    }   
}
