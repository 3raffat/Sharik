namespace Sharik.Api
{
    public static class Middlewares
    {
        public static IApplicationBuilder UseCoreMiddlewares(this IApplicationBuilder app)
        {
            app.UseExceptionHandler();

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseAuthorization();

            return app;
        }
    }
}
