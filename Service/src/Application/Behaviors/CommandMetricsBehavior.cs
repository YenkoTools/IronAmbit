using Application.Common;
using Domain.Common;
using Microsoft.Extensions.Logging;

namespace Application.Behaviors;

/// <summary>
/// Pipeline behavior that collects business metrics for commands.
/// Complements CommandPerformanceBehavior by focusing on success/failure rates and business intelligence.
/// </summary>
/// <typeparam name="TCommand">The type of command being processed.</typeparam>
/// <typeparam name="TCommandResult">The type of result returned by the command.</typeparam>
public class CommandMetricsBehavior<TCommand, TCommandResult> : ICommandPipelineBehavior<TCommand, TCommandResult>
    where TCommandResult : class
{
    private readonly ILogger<CommandMetricsBehavior<TCommand, TCommandResult>> _logger;
    private readonly IMetricsService _metrics;

    public CommandMetricsBehavior(
        ILogger<CommandMetricsBehavior<TCommand, TCommandResult>> logger,
        IMetricsService metrics)
    {
        _logger = logger;
        _metrics = metrics;
    }

    public async Task<TCommandResult> Handle(TCommand command, CancellationToken cancellationToken, Func<Task<TCommandResult>> next)
    {
        var commandName = typeof(TCommand).Name.Replace("Command", "").ToLowerInvariant();

        var tags = new Dictionary<string, string>
        {
            ["CommandType"] = commandName,
            ["Operation"] = "command"
        };

        try
        {
            // Record business metric: command attempt
            _metrics.RecordCounter($"{commandName}_attempts", tags);
            _metrics.RecordCounter("command_attempts_total", tags);

            _logger.LogDebug("Recording metrics for command attempt: {CommandName}", commandName);

            var result = await next();

            // Analyze result for business success/failure
            var isSuccess = IsSuccessResult(result);
            var outcome = isSuccess ? "success" : "business_failure";

            tags.Add("Outcome", outcome);

            // Record business metrics: outcome
            _metrics.RecordCounter($"{commandName}_{outcome}", tags);
            _metrics.RecordCounter($"command_{outcome}_total", tags);

            _logger.LogInformation("BUSINESS_METRIC: Command {CommandName} completed with outcome: {Outcome}",
                commandName, outcome);

            return result;
        }
        catch (Exception ex)
        {
            // Record business metric: exception
            tags.Add("Outcome", "exception");
            tags.Add("ExceptionType", ex.GetType().Name);

            _metrics.RecordCounter($"{commandName}_exceptions", tags);
            _metrics.RecordCounter("command_exceptions_total", tags);

            _logger.LogWarning("BUSINESS_METRIC: Command {CommandName} threw {ExceptionType}",
                commandName, ex.GetType().Name);

            throw;
        }
    }

    private static bool IsSuccessResult(TCommandResult result)
    {
        // Handle Result<T> pattern
        if (result.GetType().IsGenericType && result.GetType().GetGenericTypeDefinition() == typeof(Result<>))
        {
            var isSuccessProperty = result.GetType().GetProperty("IsSuccess");
            return (bool)(isSuccessProperty?.GetValue(result) ?? false);
        }

        // Handle other common success patterns
        if (result.GetType().Name.Contains("Result"))
        {
            var isSuccessProperty = result.GetType().GetProperty("IsSuccess") ??
                                   result.GetType().GetProperty("Success") ??
                                   result.GetType().GetProperty("Succeeded");
            if (isSuccessProperty != null)
            {
                return (bool)(isSuccessProperty.GetValue(result) ?? false);
            }
        }

        // Default: non-null result is considered success
        return result != null;
    }
}
