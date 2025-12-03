namespace Infrastructure.Configuration;

/// <summary>
/// Represents the connection strings configuration options.
/// </summary>
public class ConnectionStringsOptions
{
    /// <summary>
    /// The configuration section name for connection strings.
    /// </summary>
    public const string SectionName = "ConnectionStrings";

    /// <summary>
    /// Gets or sets the default database connection string.
    /// </summary>
    public string DefaultConnection { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the database provider (e.g., "SqlServer", "Sqlite").
    /// </summary>
    public string Provider { get; set; } = "Sqlite";
}
