using Domain.Entities;

namespace Application.Interfaces;

/// <summary>
/// Defines the contract for the exercise repository.
/// </summary>
public interface IExerciseRepository : IRepository<Exercise>
{
    /// <summary>
    /// Gets exercises by category.
    /// </summary>
    /// <param name="category">The exercise category.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of exercises in the specified category.</returns>
    Task<IEnumerable<Exercise>> GetByCategoryAsync(string category, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets exercises by muscle group.
    /// </summary>
    /// <param name="muscleGroup">The muscle group.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of exercises targeting the specified muscle group.</returns>
    Task<IEnumerable<Exercise>> GetByMuscleGroupAsync(string muscleGroup, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets exercises by difficulty level.
    /// </summary>
    /// <param name="difficulty">The difficulty level.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of exercises with the specified difficulty level.</returns>
    Task<IEnumerable<Exercise>> GetByDifficultyAsync(string difficulty, CancellationToken cancellationToken = default);
}
