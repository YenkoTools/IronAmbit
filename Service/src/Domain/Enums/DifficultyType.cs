using Ardalis.SmartEnum;

namespace Domain.Enums;

/// <summary>
/// Represents the difficulty level of an exercise.
/// </summary>
public class DifficultyType : SmartEnum<DifficultyType>
{
    /// <summary>
    /// Beginner difficulty level for exercises suitable for novices.
    /// </summary>
    public static readonly DifficultyType BEGINNER = new DifficultyType(nameof(BEGINNER), "Beginner", 1);
    
    /// <summary>
    /// Intermediate difficulty level for exercises requiring some experience.
    /// </summary>
    public static readonly DifficultyType INTERMEDIATE = new DifficultyType(nameof(INTERMEDIATE), "Intermediate", 2);
    
    /// <summary>
    /// Advanced difficulty level for exercises requiring significant experience and skill.
    /// </summary>
    public static readonly DifficultyType ADVANCED = new DifficultyType(nameof(ADVANCED), "Advanced", 3);

    /// <summary>
    /// Gets the human-readable display name for the difficulty level.
    /// </summary>
    public string DisplayName { get; }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="DifficultyType"/> class.
    /// </summary>
    /// <param name="name">The internal name of the difficulty type.</param>
    /// <param name="displayName">The display name of the difficulty type.</param>
    /// <param name="value">The numeric value representing the difficulty level.</param>
    protected DifficultyType(string name, string displayName, int value) : base(name, value)
    {
        DisplayName = displayName;
    }
}


