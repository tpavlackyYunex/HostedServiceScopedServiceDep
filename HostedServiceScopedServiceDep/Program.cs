using HostedServiceScopedServiceDep;

var builder = WebApplication.CreateBuilder();

// Add services to the container.
builder.Services.AddScoped<INumberService, NumberService>();
builder.Services.AddHostedService<PollingHostedService>();

builder.Host.UseDefaultServiceProvider(options =>
{
    options.ValidateOnBuild = true; // Validate services during startup
    options.ValidateScopes = false; // Optional: Validate scope dependencies
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapGet("/getNumber", (INumberService randomService) => new
{
    ID = randomService.Id,
    Number = randomService.GetNextNumber(),
});

app.Run();