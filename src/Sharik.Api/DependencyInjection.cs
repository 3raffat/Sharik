using Asp.Versioning;
using Sharik.Api.OpenApi;
using Sharik.Api.Services;
using Sharik.Application.Common.Interfaces;
using System.Text.Json.Serialization;

namespace Sharik.Api
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {

            services.AddCustomApiVersioning()
                    .AddApiDocumentation()
                    .AddJsonConfiguration()
                    .AddSignalRConfiguration()
                    .AddCurrentUser()
                    .AddCors();

            return services;
        }
        public static IServiceCollection AddCustomApiVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1 , 0);
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
                services.AddOpenApi(version , options =>
                {
                    // Versioning config
                    options.AddDocumentTransformer<VersionInfoTransformer>();

                    // Security Scheme config
                    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();

                    options.AddOperationTransformer<BearerSecurityOperationTransformer>();
                });
            }
            return services;
        }
        public static IServiceCollection AddJsonConfiguration(this IServiceCollection services)
        {
            services.ConfigureHttpJsonOptions(options =>
            {
                options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });


            services.AddScoped<INotificationService , NotificationService>();

            return services;
        }
        public static IServiceCollection AddCors(this IServiceCollection services)
        {

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins("http://localhost:****")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithExposedHeaders();
                });
            });
            return services;
        }
        public static IServiceCollection AddSignalRConfiguration(this IServiceCollection services)
        {

            services.AddSignalR()
                          .AddJsonProtocol(options =>
                          {
                              options.PayloadSerializerOptions.Converters.Add(
                                  new JsonStringEnumConverter());
                          });

            return services;
        }
        public static IServiceCollection AddCurrentUser(this IServiceCollection services)
        {

            services.AddScoped<IUser , CurrentUser>();
            services.AddHttpContextAccessor();

            return services;
        }
    }
}
