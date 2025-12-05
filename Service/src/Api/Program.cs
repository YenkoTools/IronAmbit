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

var app = builder.Build();

// Enable static files and default files
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseHttpsRedirection();


app.Run();

