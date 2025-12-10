using Api.Extensions;
using Application.Common;
using Application.Features.Users.Commands;
using Application.Features.Users.Queries;
using Domain.Common;
using Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Api.Endpoints;

internal static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/users", async (
            IQueryDispatcher queryDispatcher,
            ILogger<IEndpointRouteBuilder> logger,
            int page = 1,
            int pageSize = 10) =>
        {
            try
            {
                var query = new GetUsersQuery(page, pageSize);
                var result = await queryDispatcher.Dispatch<GetUsersQuery, Result<PagedResult<User>>>(query, default);

                return result.IsSuccess 
                    ? Results.Ok(result.Value) 
                    : result.ToProblemDetails();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving users");
                return Results.Problem("Internal server error", statusCode: 500);
            }
        })
        .WithName("GetUsers")
        .WithTags("Users")
        .WithSummary("Get paginated list of users")
        .WithDescription("Retrieves a paginated list of all users in the system")
        .Produces<PagedResult<User>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError);

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
        .WithTags("Users")
        .WithSummary("Create a new user")
        .WithDescription("Creates a new user.")
        .Produces<User>(StatusCodes.Status201Created)
        .ProducesValidationProblem()
        .ProducesProblem(StatusCodes.Status500InternalServerError);

        return app;
    }
}