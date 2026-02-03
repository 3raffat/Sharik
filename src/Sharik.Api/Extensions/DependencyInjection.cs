using Asp.Versioning;
using Sharik.Api.OpenApi;
using Sharik.Api.Services;
using Sharik.Application.Common.Interfaces;

namespace Sharik.Api.Extensions
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
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            }).AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
            return services;
        }

        public static IServiceCollection AddApiDocumentation(this IServiceCollection services)
        {
            string[] versions = ["v1"];
            foreach (var version in versions)
            {
                services.AddOpenApi(version, options =>
                {
                    //Versioning config
                    options.AddDocumentTransformer<VersionInfoTransformer>();

                    //Security Scheme config
                    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
                    options.AddOperationTransformer<BearerSecuritySchemeTransformer>();
                });
            }
            return services;
        }
    }
}
