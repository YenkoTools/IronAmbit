using Api.Endpoints;
using Api.Extensions;
using Application.Extensions;
using Infrastructure.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

// Add configuration sources
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables()
    .AddUserSecrets<Program>();

// Configure Serilog with custom enrichments
// Api.Extensions.LoggingExtensions
builder.ConfigureCustomSerilog();

// Add Infrastructure services (DbContext, Repositories)
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplicationServices();

// Add Database health checks
builder.Services.AddDatabaseHealthChecks();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowClient", policy =>
    {
        policy.WithOrigins("http://localhost:4321")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Add OpenAPI/Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "IronAmbit API",
        Version = "v1",
        Description = "RESTful API for IronAmbit fitness tracking application"
    });
});

var app = builder.Build();

// Configure Swagger/OpenAPI in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "IronAmbit API v1");
        options.RoutePrefix = "swagger";
    });
}

// Enable static files and default files
app.UseDefaultFiles();
app.UseStaticFiles();

// Enable CORS
app.UseCors("AllowClient");

app.UseHttpsRedirection();

// Map API endpoints
app.MapUserEndpoints();

app.Run();

