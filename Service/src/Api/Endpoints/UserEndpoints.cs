using Api.Extensions;
using Application.Common;
using Application.Features.Users.Commands;
using Domain.Common;
using Domain.Entities;
using FluentValidation;

namespace Api.Endpoints;

internal static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/users", async (CreateUserCommand command, ICommandDispatcher commandDispatcher, ILogger<IEndpointRouteBuilder> logger) =>
        {
            try
            {
                var result = await commandDispatcher.Dispatch<CreateUserCommand, Result<User>>(command, default);
                return result.IsSuccess ? Results.Created($"/api/users/{result.Value.Id}", result.Value) : result.ToProblemDetails();

            }
            catch (ValidationException validationEx)
            {
                logger.LogWarning("Validation failed for CreateUserCommand: {ValidationErrors}",
                    string.Join("; ", validationEx.Errors.Select(e => e.ErrorMessage)));

                return Results.ValidationProblem(
                    validationEx.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray()),
                    title: "One or more validation errors occurred");
            }

            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while creating user");
                return Results.Problem("Internal server error", statusCode: 500);
            }

        })
        .WithName("CreateUser")
        .WithSummary("Create a new user")
        .WithDescription("Creates a new user.")
        .WithTags("Users");

        return app;
    }
}