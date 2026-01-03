# OpenTelemetry Quick Start Guide

## ✅ Integration Complete!

OpenTelemetry has been successfully integrated into your IronAmbit Service layer.

## What Was Integrated

### 1. **Behaviors Enhanced**
- ✅ [`CommandMetricsBehavior`](/home/jim/Projects/IronAmbit/Service/src/Application/Behaviors/CommandMetricsBehavior.cs) - Now includes OpenTelemetry tracing and metrics
- ✅ [`QueryMetricsBehavior`](/home/jim/Projects/IronAmbit/Service/src/Application/Behaviors/QueryMetricsBehavior.cs) - Now includes OpenTelemetry tracing and metrics

### 2. **Configuration Added**
- ✅ [`appsettings.Development.json`](/home/jim/Projects/IronAmbit/Service/src/Api/appsettings.Development.json) - OpenTelemetry configuration for Aspire Dashboard
- ✅ [`Program.cs`](/home/jim/Projects/IronAmbit/Service/src/Api/Program.cs) - OpenTelemetry services registration

### 3. **Packages Installed**
- OpenTelemetry 1.11.0
- OpenTelemetry.Exporter.OpenTelemetryProtocol 1.11.0
- OpenTelemetry.Extensions.Hosting 1.11.0
- OpenTelemetry.Instrumentation.AspNetCore 1.11.0
- OpenTelemetry.Instrumentation.Http 1.11.0
- OpenTelemetry.Instrumentation.Runtime 1.11.0
- OpenTelemetry.Instrumentation.EntityFrameworkCore 1.10.0-beta.1

## Running Your Application with OpenTelemetry

### Step 1: Start Aspire Dashboard (Already Running)

You mentioned you're already running the Aspire collector locally. If you need to verify it's running:

```bash
docker ps | grep aspire
```

If it's not running, start it with:

```bash
docker run -d --name aspire-dashboard \
  -p 18888:18888 \
  -p 18889:18889 \
  -p 18890:18890 \
  -e DOTNET_DASHBOARD_UNSECURED_ALLOW_ANONYMOUS=true \
  mcr.microsoft.com/dotnet/aspire-dashboard:latest
```

### Step 2: Run Your Service

```bash
cd /home/jim/Projects/IronAmbit/Service
dotnet run --project src/Api
```

### Step 3: View Telemetry

Open your browser to: **http://localhost:18888**

You'll see:
- **Traces**: Distributed traces for each HTTP request
- **Metrics**: Command/Query metrics and infrastructure metrics
- **Logs**: Structured logs (if configured)

### Step 4: Generate Some Data

Make a few API requests to see telemetry:

```bash
# Get users
curl http://localhost:5000/api/users

# Create a user (if endpoint exists)
curl -X POST http://localhost:5000/api/users \
  -H "Content-Type: application/json" \
  -d '{"name":"Test User","email":"test@example.com"}'
```

## What You'll See in Aspire Dashboard

### Traces Tab
Each HTTP request will show:
- **Root Span**: ASP.NET Core HTTP request
- **Child Spans**: 
  - `Query.getusers` or `Command.createuser` (from your behaviors)
  - Database queries (from EF Core instrumentation)
- **Tags**: 
  - `command.type` / `query.type`
  - `command.outcome` / `query.outcome`
  - `query.result_type`
  - `query.item_count` (for collections)

### Metrics Tab
You'll see:
- **Command Metrics**:
  - `ironambit.command.attempts` - Total attempts
  - `ironambit.command.success` - Successful commands
  - `ironambit.command.failures` - Failed commands
  - `ironambit.command.exceptions` - Command exceptions
  
- **Query Metrics**:
  - `ironambit.query.attempts` - Total attempts
  - `ironambit.query.success` - Successful queries
  - `ironambit.query.not_found` - Queries with no results
  - `ironambit.query.exceptions` - Query exceptions
  - `ironambit.query.result_count` - Histogram of result counts

- **Infrastructure Metrics**:
  - ASP.NET Core request rates, duration
  - HTTP client metrics
  - .NET Runtime metrics (GC, thread pool, etc.)

## Verifying the Integration

### 1. Check Traces

In the Aspire Dashboard, go to **Traces** and look for:
- A trace for each HTTP request
- Spans showing your command/query names
- Proper parent-child relationships

### 2. Check Metrics

In the Aspire Dashboard, go to **Metrics** and look for:
- `ironambit.command.*` metrics
- `ironambit.query.*` metrics
- Standard `http.server.*` metrics

### 3. Check Activity Tags

Click on a trace span and verify you see tags like:
- `command.type: createuser`
- `command.outcome: success`
- `query.result_type: collection`

## Troubleshooting

### No Traces Appearing

**Check 1: Verify OTLP Endpoint**
```bash
# In appsettings.Development.json, verify:
"OpenTelemetry": {
  "Otlp": {
    "Endpoint": "http://localhost:18889"  # Should match Aspire port
  }
}
```

**Check 2: Verify Aspire Dashboard is Running**
```bash
docker ps | grep aspire
# Should show container on ports 18888:18888, 18889:18889
```

**Check 3: Check Application Logs**
```bash
# Look for any OpenTelemetry errors in the console output
```

### Metrics Not Showing

Metrics may take a few seconds to appear. Refresh the dashboard and make sure you're looking at the correct time range.

### Build Warnings About OpenTelemetry.Api

The warning `NU1902` about OpenTelemetry.Api having a vulnerability is known and will be fixed in version 1.12.0. This is acceptable for development but should be addressed before production deployment.

## Key Features

### ✨ Automatic Instrumentation

Your behaviors now automatically:
1. Create distributed traces for each command/query
2. Record metrics for attempts, successes, failures
3. Add rich contextual tags
4. Capture exception details

### ✨ Business Intelligence

The integration preserves your existing business logic:
- Success/failure detection using your `Result<T>` pattern
- Collection counting for query results
- Exception tracking with types

### ✨ Backward Compatible

Your existing `IMetricsService` continues to work alongside OpenTelemetry, allowing for a gradual migration.

## Next Steps

1. **Explore the Data**: Make various API requests and explore the traces in Aspire Dashboard
2. **Add Custom Spans**: Add more detailed spans within your command/query handlers if needed
3. **Create Dashboards**: Export metrics to Prometheus/Grafana for production monitoring
4. **Set Up Alerts**: Configure alerting based on error rates or response times
5. **Production Setup**: Plan production deployment with proper OTLP collectors

## Production Considerations

Before deploying to production:

1. **Update OpenTelemetry.Api**: Wait for version 1.12.0 to address the vulnerability
2. **Configure Sampling**: Add trace sampling to reduce overhead
3. **Secure OTLP Endpoint**: Use TLS and authentication
4. **Set Up Collector**: Deploy OpenTelemetry Collector for production
5. **Filter Sensitive Data**: Ensure no PII is logged in traces

## Resources

- [Full Documentation](OPENTELEMETRY_SETUP.md)
- [OpenTelemetry .NET Docs](https://opentelemetry.io/docs/languages/net/)
- [Aspire Dashboard Docs](https://learn.microsoft.com/en-us/dotnet/aspire/fundamentals/dashboard/overview)

---

**Questions?** Check the [full setup documentation](OPENTELEMETRY_SETUP.md) for more details.
