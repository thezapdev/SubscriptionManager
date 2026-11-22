using System.Text.Json.Serialization;
using SubscriptionManager.API.Extensions;
using SubscriptionManager.API.Middleware;
using SubscriptionManager.Application.Interfaces;
using SubscriptionManager.Application.Services;
using SubscriptionManager.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseGlobalExceptionHandling();

// Test Dependency Injection
using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider
        .GetRequiredService<ISubscriptionService>();

    Console.WriteLine(
        $"SubscriptionService DI: {service.GetType().Name}");
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();