# OpenTelemetry Integration Guide

This document describes the OpenTelemetry integration in the IronAmbit Service layer.

## ⚠️ Known Issues

**OpenTelemetry.Api Vulnerability (NU1902)**: The current version (1.11.x) has a known moderate severity vulnerability ([GHSA-8785-wc3w-h8q6](https://github.com/advisories/GHSA-8785-wc3w-h8q6)). This will be resolved in version 1.12.0 when released. For development purposes, this warning can be suppressed, but should be addressed before production deployment.

## Overview

OpenTelemetry has been integrated into the service layer to provide comprehensive observability through:
- **Distributed Tracing**: Track requests end-to-end across the application
- **Metrics**: Monitor business and infrastructure metrics
- **Activity Sources**: Custom instrumentation for Commands and Queries

## Architecture

### Instrumentation Points

1. **Command Pipeline** (`CommandMetricsBehavior`)
   - Activity Source: `IronAmbit.Application.Commands`
   - Meter: `IronAmbit.Application.Commands`
   - Tracks: attempts, success, failures, exceptions

2. **Query Pipeline** (`QueryMetricsBehavior`)
   - Activity Source: `IronAmbit.Application.Queries`
   - Meter: `IronAmbit.Application.Queries`
   - Tracks: attempts, success, not_found, exceptions, result counts

3. **Infrastructure**
   - ASP.NET Core (HTTP requests)
   - HTTP Client (outbound requests)
   - Entity Framework Core (database queries)
   - Runtime metrics (GC, thread pool, etc.)

## Configuration

### Development Environment (appsettings.Development.json)

```json
{
  "OpenTelemetry": {
    "ServiceName": "IronAmbit.Service",
    "ServiceVersion": "1.0.0",
    "Otlp": {
      "Endpoint": "http://localhost:18889"
    }
  }
}
```

### Running Aspire Dashboard (Docker)

```bash
# Pull and run the Aspire Dashboard
docker run -d --name aspire-dashboard \
  -p 18888:18888 \
  -p 18889:18889 \
  -e DOTNET_DASHBOARD_UNSECURED_ALLOW_ANONYMOUS=true \
  mcr.microsoft.com/dotnet/aspire-dashboard:latest

# View the dashboard at:
# http://localhost:18888
```

### Alternative: Using Jaeger

```bash
# Run Jaeger all-in-one
docker run -d --name jaeger \
  -e COLLECTOR_OTLP_ENABLED=true \
  -p 16686:16686 \
  -p 4317:4317 \
  -p 4318:4318 \
  jaegertracing/all-in-one:latest

# Update appsettings.Development.json endpoint to:
# "Endpoint": "http://localhost:4317"

# View traces at: http://localhost:16686
```

## Metrics Collected

### Command Metrics

| Metric Name | Type | Description | Tags |
|-------------|------|-------------|------|
| `ironambit.command.attempts` | Counter | Total command attempts | `command.type`, `operation` |
| `ironambit.command.success` | Counter | Successful commands | `command.type`, `operation`, `outcome` |
| `ironambit.command.failures` | Counter | Failed commands | `command.type`, `operation`, `outcome` |
| `ironambit.command.exceptions` | Counter | Command exceptions | `command.type`, `operation`, `outcome`, `exception.type` |

### Query Metrics

| Metric Name | Type | Description | Tags |
|-------------|------|-------------|------|
| `ironambit.query.attempts` | Counter | Total query attempts | `query.type`, `operation` |
| `ironambit.query.success` | Counter | Successful queries | `query.type`, `operation`, `outcome`, `result.type` |
| `ironambit.query.not_found` | Counter | Queries with no results | `query.type`, `operation`, `outcome`, `result.type` |
| `ironambit.query.exceptions` | Counter | Query exceptions | `query.type`, `operation`, `outcome`, `exception.type` |
| `ironambit.query.result_count` | Histogram | Number of items returned | `query.type`, `operation`, `result.type`, `item.count` |

## Activity Tags

### Commands

- `command.type`: The name of the command (e.g., `createuser`, `updateuser`)
- `command.fullname`: Full type name including namespace
- `operation`: Always `"command"`
- `command.outcome`: Either `"success"` or `"business_failure"`

### Queries

- `query.type`: The name of the query (e.g., `getuser`, `listusers`)
- `query.fullname`: Full type name including namespace
- `operation`: Always `"query"`
- `query.outcome`: `"success"`, `"not_found"`, or `"exception"`
- `query.result_type`: `"entity"`, `"collection"`, `"primitive"`, or `"null"`
- `query.item_count`: Number of items (for collections only)

## Verification

### 1. Start the Application

```bash
cd /home/jim/Projects/IronAmbit/Service
dotnet run --project src/Api
```

### 2. Make Some Requests

```bash
# Get users
curl http://localhost:5000/api/users

# Create a user
curl -X POST http://localhost:5000/api/users \
  -H "Content-Type: application/json" \
  -d '{"name":"Test User","email":"test@example.com"}'
```

### 3. View Telemetry

Open the Aspire Dashboard at http://localhost:18888 and navigate to:
- **Traces**: See distributed traces for each request
- **Metrics**: View command/query metrics and infrastructure metrics
- **Logs**: View structured logs (if configured)

## Troubleshooting

### No Data in Dashboard

1. Check that the OTLP endpoint is correct in appsettings.Development.json
2. Verify the Aspire Dashboard container is running: `docker ps`
3. Check application logs for OpenTelemetry errors
4. Ensure the endpoint URL matches the dashboard port (default: 18889)

### Missing Traces

1. Verify Activity Sources are registered in Program.cs:
   - `AddSource("IronAmbit.Application.Commands")`
   - `AddSource("IronAmbit.Application.Queries")`
2. Check that behaviors are registered in the DI container

### Missing Metrics

1. Verify Meters are registered in Program.cs:
   - `AddMeter("IronAmbit.Application.Commands")`
   - `AddMeter("IronAmbit.Application.Queries")`
2. Ensure metrics are being recorded in behavior methods

## Production Considerations

### Configuration

For production, consider:
1. Using a dedicated OpenTelemetry Collector
2. Exporting to multiple backends (Jaeger, Prometheus, etc.)
3. Sampling traces to reduce overhead
4. Filtering sensitive data from spans

### Example Production Config

```json
{
  "OpenTelemetry": {
    "ServiceName": "IronAmbit.Service",
    "ServiceVersion": "1.0.0",
    "Otlp": {
      "Endpoint": "http://otel-collector:4317"
    },
    "Sampling": {
      "Type": "TraceIdRatioBased",
      "Probability": 0.1
    }
  }
}
```

### Security

- Use TLS for OTLP exports in production
- Implement authentication for the collector endpoint
- Filter sensitive data from activities and metrics
- Set up proper network policies

## Integration with Existing Metrics

The behaviors continue to work with the existing `IMetricsService` for backward compatibility while adding OpenTelemetry instrumentation. This allows for a gradual migration:

1. **Legacy metrics** continue to work through `IMetricsService`
2. **OpenTelemetry metrics** are collected in parallel
3. Both systems can coexist during the transition period

## Next Steps

1. **Add Sampling**: Configure trace sampling for production
2. **Custom Spans**: Add more detailed spans within command/query handlers
3. **Dashboards**: Create Grafana dashboards for metrics
4. **Alerts**: Set up alerting based on metrics thresholds
5. **Correlation**: Correlate logs with traces using trace IDs

## References

- [OpenTelemetry .NET Documentation](https://opentelemetry.io/docs/languages/net/)
- [Aspire Dashboard Documentation](https://learn.microsoft.com/en-us/dotnet/aspire/fundamentals/dashboard/overview)
- [OTLP Specification](https://opentelemetry.io/docs/specs/otlp/)
