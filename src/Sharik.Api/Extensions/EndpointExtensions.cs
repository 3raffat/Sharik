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
            app.MapExchangeEndpoints(apiVersionSet);
            app.MapRatingEndpoints(apiVersionSet);
            app.MapUserSkillEndpoints(apiVersionSet);
            app.MapCategoryEndpoints(apiVersionSet);
            app.MapSkillEndpoints(apiVersionSet);

            return app;
        }

    }
}
