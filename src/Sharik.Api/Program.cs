using Sharik.Api.Endpoints;
using Sharik.Api.Extensions;
using Sharik.Application;
using Sharik.Infrastructure;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);

builder.Services.AddPresentation()
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", " Sharik API V1");
        options.RoutePrefix = string.Empty;
        options.EnableDeepLinking();
        options.DisplayRequestDuration();
        options.EnableFilter();

    });
    await app.InitialiseDatabaseAsync();
}
using (var scope = app.Services.CreateScope())
{
    var httpContextAccessor = scope.ServiceProvider
        .GetRequiredService<IHttpContextAccessor>();
}
app.UseCoreMiddlewares();

app.MapAllEndpoints();

app.Run();

