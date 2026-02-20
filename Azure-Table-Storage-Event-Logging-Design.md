# Azure Table Storage -- Application Event Logging Design

Generated: 2026-02-20T03:01:27.203889 UTC

------------------------------------------------------------------------

## 1. Design Goals

-   High write throughput
-   Time-based querying
-   Horizontal scalability
-   Cost efficiency
-   Append-only event logging

------------------------------------------------------------------------

## 2. PartitionKey Strategy (Critical)

### Recommended Format

    {ServiceName}:{Environment}:{yyyyMMddHH}

### Example

    orders-api:prod:20260219T14

### Why This Works

-   Prevents hot partitions
-   Enables time-based queries
-   Supports scalable ingestion
-   Enables efficient retention cleanup

Hourly partitions are recommended for moderate to high volume systems.
Daily partitions may be acceptable for low-volume services.

------------------------------------------------------------------------

## 3. RowKey Strategy

### Recommended Format

    {Ticks}-{Guid}

### Example

    638441789012345678-7c9a7f5c1a6e4b34a3eaa09d0c7a1f2e

### Benefits

-   Naturally ordered by time
-   Globally unique
-   Safe under concurrent writes
-   Supports efficient point lookups

------------------------------------------------------------------------

## 4. Table Entity Schema

``` csharp
public sealed class AppEventEntity : ITableEntity
{
    public string PartitionKey { get; set; } = default!;
    public string RowKey { get; set; } = default!;
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }

    // Custom properties
    public string EventType { get; set; } = default!;
    public string PayloadJson { get; set; } = default!;
    public string Level { get; set; } = default!; // Info | Warning | Error
    public string Service { get; set; } = default!;
    public string Environment { get; set; } = default!;
    public string CorrelationId { get; set; } = default!;
}
```

------------------------------------------------------------------------

## 5. EventType Guidelines

Keep short and normalized.

Examples:

-   OrderCreated
-   PaymentFailed
-   UserAuthenticated

Avoid long or dynamic strings.

------------------------------------------------------------------------

## 6. PayloadJson Guidelines

-   Store raw JSON
-   Keep payloads under 1MB (Azure Table entity limit)
-   Avoid deeply nested structures
-   For large payloads, store content in Blob Storage and save the Blob
    URI in the table

------------------------------------------------------------------------

## 7. Example: Writing an Event

``` csharp
public async Task LogEventAsync(
    TableClient table,
    string service,
    string environment,
    string eventType,
    object payload,
    string level,
    string correlationId)
{
    var now = DateTimeOffset.UtcNow;

    var entity = new AppEventEntity
    {
        PartitionKey = $"{service}:{environment}:{now:yyyyMMddHH}",
        RowKey = $"{now.Ticks}-{Guid.NewGuid():N}",
        EventType = eventType,
        PayloadJson = JsonSerializer.Serialize(payload),
        Level = level,
        Service = service,
        Environment = environment,
        CorrelationId = correlationId
    };

    await table.AddEntityAsync(entity);
}
```

------------------------------------------------------------------------

## 8. Supported Query Patterns

### Time Range Query

Filter by PartitionKey range:

    PartitionKey ge 'orders-api:prod:20260219T10'
    AND PartitionKey le 'orders-api:prod:20260219T12'

### Point Lookup

    PartitionKey == ...
    AND RowKey == ...

------------------------------------------------------------------------

## 9. Retention Strategy

-   Use time-based PartitionKeys for efficient deletion
-   Schedule cleanup jobs by partition
-   Optionally use Azure lifecycle policies if integrating with Blob
    storage

------------------------------------------------------------------------

## 10. When to Consider Alternatives

Use other Azure services if you require:

-   Complex analytics → Azure Data Explorer
-   Full-text search → Azure Log Analytics
-   Metrics & observability dashboards → Azure Monitor

------------------------------------------------------------------------

## Final Design Summary

  Component      Recommendation
  -------------- --------------------------------
  PartitionKey   Service:Environment:yyyyMMddHH
  RowKey         Ticks-Guid
  EventType      Short, normalized string
  PayloadJson    Raw JSON (\< 1MB)
  Model          Append-only
  Retention      Time-based cleanup
  Scalability    High write throughput

------------------------------------------------------------------------

End of Document
