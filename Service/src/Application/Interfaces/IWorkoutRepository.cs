using Domain.Entities;

namespace Application.Interfaces;

/// <summary>
/// Defines the contract for the workout repository.
/// </summary>
public interface IWorkoutRepository : IRepository<Workout>
{
    /// <summary>
    /// Gets workouts for a specific user.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of workouts for the specified user.</returns>
    Task<IEnumerable<Workout>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets workouts for a specific exercise.
    /// </summary>
    /// <param name="exerciseId">The exercise identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of workouts for the specified exercise.</returns>
    Task<IEnumerable<Workout>> GetByExerciseIdAsync(int exerciseId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets workouts within a date range.
    /// </summary>
    /// <param name="startDate">The start date.</param>
    /// <param name="endDate">The end date.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of workouts within the specified date range.</returns>
    Task<IEnumerable<Workout>> GetByDateRangeAsync(DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets workouts for a specific user within a date range.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <param name="startDate">The start date.</param>
    /// <param name="endDate">The end date.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of workouts for the specified user within the date range.</returns>
    Task<IEnumerable<Workout>> GetByUserIdAndDateRangeAsync(int userId, DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken = default);
}
