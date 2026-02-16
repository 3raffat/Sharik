using Scalar.AspNetCore;
using Sharik.Api;
using Sharik.Api.Extensions;
using Sharik.Api.Hubs;
using Sharik.Application;
using Sharik.Infrastructure;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Configuration.AddJsonFile("appsettings.Local.json" , optional: true , reloadOnChange: true);

builder.Services.AddPresentation()
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference();
    app.UseDeveloperExceptionPage();
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json" , " Sharik API V1");
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

app.UseCors();

app.UseCoreMiddlewares();

app.MapAllEndpoints();

app.MapHub<NotificationHub>("/notificationHub");
app.MapHub<ChatHub>("/chatHub");

app.Run();

