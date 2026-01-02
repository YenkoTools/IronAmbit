# Aspire Dashboard Setup

## Overview

The Aspire Dashboard provides a web-based UI for viewing OpenTelemetry data (traces, metrics, and logs) from the IronAmbit Service.

## Prerequisites

- Docker installed and running
- Ports 18888, 18889, and 18890 available

## Starting the Aspire Dashboard

Run the following command to start the Aspire Dashboard:

```bash
docker run -d --name aspire-dashboard \
  -p 18888:18888 \
  -p 18889:18889 \
  -p 18890:18890 \
  -e DOTNET_DASHBOARD_UNSECURED_ALLOW_ANONYMOUS=true \
  mcr.microsoft.com/dotnet/aspire-dashboard:latest
```

### Port Descriptions

- **18888**: Dashboard web UI (HTTP)
- **18889**: OTLP gRPC endpoint
- **18890**: OTLP HTTP endpoint (used by IronAmbit Service)

### Access the Dashboard

Once started, open your browser to:

**http://localhost:18888**

## Stopping the Aspire Dashboard

```bash
docker stop aspire-dashboard
```

## Starting an Existing Container

If the container already exists but is stopped:

```bash
docker start aspire-dashboard
```

## Removing the Container

To completely remove the container:

```bash
docker rm aspire-dashboard
```

Or to stop and remove in one command:

```bash
docker rm -f aspire-dashboard
```

## Viewing Logs

To view the dashboard container logs:

```bash
docker logs aspire-dashboard
```

To follow logs in real-time:

```bash
docker logs -f aspire-dashboard
```

## Checking Status

To check if the container is running:

```bash
docker ps | grep aspire-dashboard
```

To view all containers (including stopped ones):

```bash
docker ps -a | grep aspire-dashboard
```

## Restarting the Dashboard

```bash
docker restart aspire-dashboard
```

## Environment Variables

### Required
- `DOTNET_DASHBOARD_UNSECURED_ALLOW_ANONYMOUS=true` - Enables anonymous access to OTLP endpoints (development only)

### Optional
- `ASPNETCORE_URLS` - Override the dashboard UI URL (default: `http://+:18888`)
- `DOTNET_DASHBOARD_OTLP_ENDPOINT_URL` - Override OTLP gRPC endpoint (default: `http://+:18889`)
- `DOTNET_DASHBOARD_OTLP_HTTP_ENDPOINT_URL` - Override OTLP HTTP endpoint (default: `http://+:18890`)

## Troubleshooting

### Port Already in Use

If you get a port conflict error, check what's using the port:

```bash
sudo lsof -i :18888
```

### Container Won't Start

Check the logs for errors:

```bash
docker logs aspire-dashboard
```

### No Data Appearing

1. Verify the container is running: `docker ps | grep aspire-dashboard`
2. Verify the service is sending to the correct endpoint (`http://localhost:18890`)
3. Check the dashboard logs for incoming requests
4. Ensure the environment variable `DOTNET_DASHBOARD_UNSECURED_ALLOW_ANONYMOUS=true` is set

## Production Considerations

⚠️ **WARNING**: The configuration above is for **development only**.

For production:
1. Remove `DOTNET_DASHBOARD_UNSECURED_ALLOW_ANONYMOUS=true`
2. Configure proper authentication
3. Use TLS/SSL for all endpoints
4. Run behind a reverse proxy
5. Consider using a proper OpenTelemetry Collector

## References

- [Aspire Dashboard Documentation](https://learn.microsoft.com/en-us/dotnet/aspire/fundamentals/dashboard/overview)
- [OpenTelemetry Documentation](https://opentelemetry.io/docs/)
