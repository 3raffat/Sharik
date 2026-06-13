using Asp.Versioning;
using Sharik.Api.Extensions;
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
                    .AddJsonConfiguration()
                    .AddCustomProblemDetails()
                    .AddExceptionHandling()
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
                    policy.WithOrigins("http://localhost:3000")
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
        public static IServiceCollection AddCustomProblemDetails(this IServiceCollection services)
        {
            services.AddProblemDetails(options => options.CustomizeProblemDetails = (context) =>
            {
                context.ProblemDetails.Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
                context.ProblemDetails.Extensions.Add("requestId" , context.HttpContext.TraceIdentifier);
            });

            return services;
        }
        public static IServiceCollection AddExceptionHandling(this IServiceCollection services)
        {
            services.AddExceptionHandler<GlobalExceptionHandler>();
            return services;
        }
    }
}
