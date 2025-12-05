namespace Domain.Entities;

/// <summary>
/// Represents a workout record in the system.
/// </summary>
public class Workout : Entity
{
    /// <summary>
    /// Gets or sets the ID of the user who performed the workout.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Gets or sets the name of the user who performed the workout.
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date when the workout was performed.
    /// </summary>
    public DateOnly Date { get; set; }

    /// <summary>
    /// Gets or sets the ID of the exercise performed in the workout.
    /// </summary>
    public int ExerciseId { get; set; }

    /// <summary>
    /// Gets or sets the name of the exercise performed in the workout.
    /// </summary>
    public string ExerciseName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the number of sets performed.
    /// </summary>
    public int Sets { get; set; }

    /// <summary>
    /// Gets or sets the number of repetitions per set.
    /// </summary>
    public int Reps { get; set; }

    /// <summary>
    /// Gets or sets the weight used in pounds or kilograms.
    /// </summary>
    public decimal Weight { get; set; }

    /// <summary>
    /// Gets or sets the duration of the workout in minutes.
    /// </summary>
    public int Duration { get; set; }

    /// <summary>
    /// Gets or sets additional notes about the workout.
    /// </summary>
    public string Notes { get; set; } = string.Empty;
}
