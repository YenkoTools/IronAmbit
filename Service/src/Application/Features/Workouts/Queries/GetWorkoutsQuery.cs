namespace Application.Features.Workouts.Queries;

public record GetWorkoutsQuery(int PageNumber = 1, int PageSize = 10);