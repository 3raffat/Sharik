using Sharik.Api;
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
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", " Sharik API V1");

        options.EnableDeepLinking();
        options.DisplayRequestDuration();
        options.EnableFilter();

    });
}

app.UseHttpsRedirection();

app.Run();

