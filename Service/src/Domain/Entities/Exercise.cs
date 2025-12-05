namespace Domain.Entities;

/// <summary>
/// Represents an exercise in the system.
/// </summary>
public class Exercise : Entity
{
    /// <summary>
    /// Gets or sets the name of the exercise.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the exercise.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the category of the exercise (e.g., Strength, Cardio).
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the primary muscle group targeted by the exercise.
    /// </summary>
    public string MuscleGroup { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the equipment required for the exercise.
    /// </summary>
    public string Equipment { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the difficulty level of the exercise (e.g., beginner, intermediate, advanced).
    /// </summary>
    public string Difficulty { get; set; } = string.Empty;
}
