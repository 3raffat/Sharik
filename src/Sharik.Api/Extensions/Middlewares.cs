namespace Sharik.Api.Extensions
{
    public static class Middlewares
    {
        public static IApplicationBuilder UseCoreMiddlewares(this IApplicationBuilder app)
        {
            app.UseHttpsRedirection();


           

            return app;
        }
    }
}
