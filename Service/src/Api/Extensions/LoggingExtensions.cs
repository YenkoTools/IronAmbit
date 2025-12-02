using System.Text.RegularExpressions;
using Serilog;

namespace Api.Extensions;

/// <summary>
/// Provides extension methods for logging configuration.
/// </summary>
public static class LoggingExtensions
{
    /// <summary>
    /// Extracts the port number from the application's configured URLs.
    /// </summary>
    /// <param name="builder">The web application builder.</param>
    /// <returns>The port number as a string, or "unknown" if not found.</returns>
    public static string GetPortNumber(this WebApplicationBuilder builder)
    {
        var urls = builder.Configuration["urls"] ?? Environment.GetEnvironmentVariable("ASPNETCORE_URLS");

        if (!string.IsNullOrEmpty(urls))
        {
            // Extract port from URLs (e.g., "http://*:3010" or "https://localhost:3011")
            var match = Regex.Match(urls, ":([0-9]+)");
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
        }

        return "unknown";
    }

    /// <summary>
    /// Configures Serilog with port number enrichment.
    /// </summary>
    /// <param name="builder">The web application builder.</param>
    /// <returns>The web application builder for method chaining.</returns>
    public static WebApplicationBuilder ConfigureCustomSerilog(this WebApplicationBuilder builder)
    {
        var portNumber = builder.GetPortNumber();

        builder.Host.UseSerilog((context, services, configuration) =>
            configuration
                .ReadFrom.Configuration(context.Configuration)
                .Enrich.WithProperty("Port", portNumber));

        return builder;
    }
}
