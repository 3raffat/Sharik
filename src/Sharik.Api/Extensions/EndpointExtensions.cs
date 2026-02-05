using Sharik.Api.Endpoints;
using Asp.Versioning;
namespace Sharik.Api.Extensions
{
    public static class EndpointExtensions
    {
        public static IEndpointRouteBuilder MapAllEndpoints(this IEndpointRouteBuilder app)
        {
            var apiVersionSet = app.NewApiVersionSet()
                .HasApiVersion(new ApiVersion(1))
                .ReportApiVersions()
                .Build();

            app.MapAuthEndpoints(apiVersionSet);
            app.MapUserEndpoints(apiVersionSet);
            app.MapUserSkillEndpoints(apiVersionSet);
            app.MapSkillEndpoints(apiVersionSet);
            app.MapCategoryEndpoints(apiVersionSet);
            return app;
        }

    }
}
