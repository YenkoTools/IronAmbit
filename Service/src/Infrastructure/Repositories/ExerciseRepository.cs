using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

/// <summary>
/// Provides repository implementation for exercise entities.
/// </summary>
public class ExerciseRepository : Repository<Exercise>, IExerciseRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ExerciseRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public ExerciseRepository(ApplicationDbContext context) : base(context)
    {
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Exercise>> GetByCategoryAsync(string category, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(e => e.Category == category)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Exercise>> GetByMuscleGroupAsync(string muscleGroup, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(e => e.MuscleGroup == muscleGroup)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Exercise>> GetByDifficultyAsync(string difficulty, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(e => e.Difficulty == difficulty)
            .ToListAsync(cancellationToken);
    }
}
