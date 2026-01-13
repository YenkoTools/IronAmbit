using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints;

internal static class AuditEndpoints
{
    public static IEndpointRouteBuilder MapAuditEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/audits/addToQueue/{itemCount}", 
            async ([FromServices] ILocalQueueService<int> queueService, int itemCount) =>
        {
            for (int i = 1; i <= itemCount; i++)
            {
                queueService.Enqueue(Random.Shared.Next(1, 1000));
                await Task.Delay(10); // Simulate some delay
            }
        });

        return app;
    }
}