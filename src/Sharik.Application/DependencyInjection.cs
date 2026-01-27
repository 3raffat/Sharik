using Microsoft.Extensions.DependencyInjection;

namespace Sharik.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection _services)
    {
        return _services;
    }   
}
