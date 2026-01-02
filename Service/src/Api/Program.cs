using Api.Endpoints;
using Api.Extensions;
using Application.Extensions;
using Infrastructure.Extensions;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
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

// Configure OpenTelemetry
var serviceName = builder.Configuration["OpenTelemetry:ServiceName"] ?? "IronAmbit.Service";
var serviceVersion = builder.Configuration["OpenTelemetry:ServiceVersion"] ?? "1.0.0";
var otlpEndpoint = builder.Configuration["OpenTelemetry:Otlp:Endpoint"] ?? "http://localhost:18889";

Log.Information("OpenTelemetry configured with endpoint: {Endpoint}", otlpEndpoint);

// Enable detailed OTLP exporter logging
builder.Logging.SetMinimumLevel(LogLevel.Trace);
builder.Logging.AddFilter("OpenTelemetry", LogLevel.Trace);
builder.Logging.AddFilter("OpenTelemetry.Exporter", LogLevel.Trace);
builder.Logging.AddFilter("OpenTelemetry.Exporter.OpenTelemetryProtocol", LogLevel.Trace);

builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource
        .AddService(serviceName: serviceName, serviceVersion: serviceVersion)
        .AddAttributes(new Dictionary<string, object>
        {
            ["deployment.environment"] = builder.Environment.EnvironmentName,
            ["host.name"] = Environment.MachineName
        }))
    .WithTracing(tracing => tracing
        .AddAspNetCoreInstrumentation(options =>
        {
            options.RecordException = true;
            options.EnrichWithHttpRequest = (activity, httpRequest) =>
            {
                activity.SetTag("http.client_ip", httpRequest.HttpContext.Connection.RemoteIpAddress?.ToString());
            };
        })
        .AddHttpClientInstrumentation(options =>
        {
            options.RecordException = true;
        })
        .AddEntityFrameworkCoreInstrumentation(options =>
        {
            options.EnrichWithIDbCommand = (activity, command) =>
            {
                activity.SetTag("db.command_timeout", command.CommandTimeout);
            };
        })
        .AddSource("IronAmbit.Application.Commands")
        .AddSource("IronAmbit.Application.Queries")
        .AddOtlpExporter(options =>
        {
            options.Endpoint = new Uri(otlpEndpoint);
            options.Protocol = OtlpExportProtocol.HttpProtobuf;
        }))
    .WithMetrics(metrics => metrics
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddRuntimeInstrumentation()
        .AddMeter("IronAmbit.Application.Commands")
        .AddMeter("IronAmbit.Application.Queries")
        .AddOtlpExporter(options =>
        {
            options.Endpoint = new Uri(otlpEndpoint);
            options.Protocol = OtlpExportProtocol.HttpProtobuf;
        }));

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

// Add Serilog request logging
app.UseSerilogRequestLogging();

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

