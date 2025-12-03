# Entity Framework Repositories

This document provides an overview of the Entity Framework repositories implemented for the IronAmbit Service.

## Overview

The application uses Entity Framework Core with support for both SQLite and SQL Server database providers. The repository pattern is implemented following Clean Architecture principles.

## Architecture

### Layer Organization

- **Domain Layer**: Contains entity models (`User`, `Exercise`, `Workout`)
- **Application Layer**: Contains repository interfaces
- **Infrastructure Layer**: Contains repository implementations, DbContext, and EF Core configuration

### Components

#### Entities (Domain.Entities)

Base entity class with common properties:
- `Entity` - Abstract base class with `Id`, `CreatedAt`, `UpdatedAt`

Concrete entities:
- `User` - User information
- `Exercise` - Exercise definitions
- `Workout` - Workout records

#### Repository Interfaces (Application.Interfaces)

- `IRepository<T>` - Generic repository interface with CRUD operations
- `IUserRepository` - User-specific repository methods
- `IExerciseRepository` - Exercise-specific repository methods
- `IWorkoutRepository` - Workout-specific repository methods

#### Repository Implementations (Infrastructure.Repositories)

- `Repository<T>` - Generic repository implementation
- `UserRepository` - User repository implementation
- `ExerciseRepository` - Exercise repository implementation
- `WorkoutRepository` - Workout repository implementation

#### Database Context (Infrastructure.Data)

- `ApplicationDbContext` - EF Core DbContext with automatic timestamp management

#### Configuration (Infrastructure.Configuration)

- `ConnectionStringsOptions` - Connection string configuration options

## Configuration

### Database Provider Selection

Configure the database provider in `appsettings.json`:

#### SQLite (Default)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=ironambit.db",
    "Provider": "Sqlite"
  }
}
```

#### SQL Server

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=IronAmbit;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true",
    "Provider": "SqlServer"
  }
}
```

### Registering Services

In `Program.cs`:

```csharp
using Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add Infrastructure services (DbContext, Repositories)
builder.Services.AddInfrastructure(builder.Configuration);

// Add Database health checks (optional)
builder.Services.AddDatabaseHealthChecks();
```

## Usage Examples

### Injecting Repositories

```csharp
public class UserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _userRepository.GetByIdAsync(id);
    }
}
```

### Common Operations

#### Get by ID

```csharp
var user = await _userRepository.GetByIdAsync(1);
```

#### Get All

```csharp
var users = await _userRepository.GetAllAsync();
```

#### Get Paginated

```csharp
var (items, totalCount) = await _userRepository.GetPagedAsync(page: 1, pageSize: 10);
```

#### Find with Predicate

```csharp
var activeUsers = await _userRepository.FindAsync(u => u.Email.Contains("@example.com"));
```

#### Add

```csharp
var newUser = new User
{
    Username = "john_doe",
    Email = "john@example.com",
    FirstName = "John",
    LastName = "Doe"
};
await _userRepository.AddAsync(newUser);
```

#### Update

```csharp
user.Email = "newemail@example.com";
await _userRepository.UpdateAsync(user);
```

#### Delete

```csharp
await _userRepository.DeleteByIdAsync(userId);
// or
await _userRepository.DeleteAsync(user);
```

### Specialized Repository Methods

#### User Repository

```csharp
// Get by username
var user = await _userRepository.GetByUsernameAsync("john_doe");

// Get by email
var user = await _userRepository.GetByEmailAsync("john@example.com");
```

#### Exercise Repository

```csharp
// Get by category
var strengthExercises = await _exerciseRepository.GetByCategoryAsync("Strength");

// Get by muscle group
var chestExercises = await _exerciseRepository.GetByMuscleGroupAsync("Chest");

// Get by difficulty
var beginnerExercises = await _exerciseRepository.GetByDifficultyAsync("beginner");
```

#### Workout Repository

```csharp
// Get by user
var userWorkouts = await _workoutRepository.GetByUserIdAsync(userId);

// Get by exercise
var exerciseHistory = await _workoutRepository.GetByExerciseIdAsync(exerciseId);

// Get by date range
var startDate = new DateOnly(2024, 1, 1);
var endDate = new DateOnly(2024, 12, 31);
var workouts = await _workoutRepository.GetByDateRangeAsync(startDate, endDate);

// Get by user and date range
var userWorkoutsInRange = await _workoutRepository.GetByUserIdAndDateRangeAsync(
    userId, startDate, endDate);
```

## Entity Features

### Automatic Timestamps

The `ApplicationDbContext` automatically manages `CreatedAt` and `UpdatedAt` timestamps:

- **CreatedAt**: Set automatically when entity is added
- **UpdatedAt**: Updated automatically when entity is modified

### Database Configuration

#### User Entity
- Unique indexes on `Username` and `Email`
- Required fields validated
- Maximum lengths enforced

#### Exercise Entity
- Indexes on `Category`, `MuscleGroup`, and `Difficulty` for query performance
- Description field supports up to 1000 characters

#### Workout Entity
- Composite index on `UserId` and `Date` for efficient user workout queries
- Indexes on `ExerciseId` and `Date` for analytics
- `Weight` stored as decimal with precision (10, 2)
- `Date` uses `DateOnly` type for date-only values

## Database Migrations

### Creating Migrations

```bash
# Navigate to the Service directory
cd Service

# Add a migration
dotnet ef migrations add InitialCreate --project src/Infrastructure --startup-project src/Api

# Update the database
dotnet ef database update --project src/Infrastructure --startup-project src/Api
```

### Migration Commands

```bash
# List migrations
dotnet ef migrations list --project src/Infrastructure --startup-project src/Api

# Remove last migration (if not applied)
dotnet ef migrations remove --project src/Infrastructure --startup-project src/Api

# Generate SQL script
dotnet ef migrations script --project src/Infrastructure --startup-project src/Api --output migrations.sql
```

## Health Checks

The infrastructure includes Entity Framework health checks:

```csharp
builder.Services.AddDatabaseHealthChecks();
```

This adds a health check endpoint that verifies database connectivity.

## SQL Server Configuration

For production SQL Server deployments:

1. Update connection string in `appsettings.Production.json`
2. Set `Provider` to `"SqlServer"`
3. The extension includes retry logic for transient failures (5 retries, 30-second max delay)

## Best Practices

1. **Always use async methods** - All repository methods are async
2. **Use cancellation tokens** - Pass cancellation tokens for long-running operations
3. **Leverage specialized methods** - Use repository-specific methods when available
4. **Handle nullability** - Repository methods may return null when entity not found
5. **Use pagination** - For large datasets, use `GetPagedAsync` instead of `GetAllAsync`
6. **Filter at database level** - Use `FindAsync` with predicates for efficient queries

## Package Dependencies

Required NuGet packages (managed centrally in `Directory.Packages.props`):

- Microsoft.EntityFrameworkCore (10.0.0)
- Microsoft.EntityFrameworkCore.Sqlite (10.0.0)
- Microsoft.EntityFrameworkCore.SqlServer (10.0.0)
- Microsoft.EntityFrameworkCore.Design (10.0.0)
- Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore (10.0.0)
