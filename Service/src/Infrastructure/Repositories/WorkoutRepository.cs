using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

/// <summary>
/// Provides repository implementation for workout entities.
/// </summary>
public class WorkoutRepository : Repository<Workout>, IWorkoutRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WorkoutRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public WorkoutRepository(ApplicationDbContext context) : base(context)
    {
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Workout>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(w => w.UserId == userId)
            .OrderByDescending(w => w.Date)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Workout>> GetByExerciseIdAsync(int exerciseId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(w => w.ExerciseId == exerciseId)
            .OrderByDescending(w => w.Date)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Workout>> GetByDateRangeAsync(DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(w => w.Date >= startDate && w.Date <= endDate)
            .OrderByDescending(w => w.Date)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Workout>> GetByUserIdAndDateRangeAsync(int userId, DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(w => w.UserId == userId && w.Date >= startDate && w.Date <= endDate)
            .OrderByDescending(w => w.Date)
            .ToListAsync(cancellationToken);
    }
}
