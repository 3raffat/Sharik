using Asp.Versioning;
using Sharik.Api.OpenApi;
using Sharik.Api.Services;
using Sharik.Application.Common.Interfaces;

namespace Sharik.Api
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            services.AddScoped<IUser,CurrentUser>();
            services.AddHttpContextAccessor();
            services.AddCustomApiVersioning()
                .AddApiDocumentation();
                return services;
        }
        public static IServiceCollection AddCustomApiVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(_options =>
            {
                _options.AssumeDefaultVersionWhenUnspecified = true;
                _options.DefaultApiVersion = new ApiVersion(1, 0);
                _options.ReportApiVersions = true;
                _options.ApiVersionReader = new UrlSegmentApiVersionReader();
            }).AddApiExplorer(_options =>
            {
                _options.GroupNameFormat = "'v'VVV";
                _options.SubstituteApiVersionInUrl = true;
            });
            return services;
        }

        public static IServiceCollection AddApiDocumentation(this IServiceCollection services)
        {
            string[] versions = ["v1"];
            foreach (var version in versions)
            {
                services.AddOpenApi(version, _options =>
                {
                    //Versioning config
                    _options.AddDocumentTransformer<VersionInfoTransformer>();

                    //Security Scheme config

                   

                });
            }
            return services;
        }
    }
}
