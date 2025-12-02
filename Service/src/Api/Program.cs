using Api.Extensions;
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


var app = builder.Build();

// Enable static files and default files
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseHttpsRedirection();


app.Run();

