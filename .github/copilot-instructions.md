# GitHub Copilot Instructions for IronAmbit

## General Guidelines

- Always show your plan first before implementing any changes
- Follow project-specific conventions and patterns
- Maintain consistency with existing code style

---

## Client Project Guidelines

### Framework and Libraries

- **Astro**: Always use the latest version of AstroJs
- **React**: Always use the latest version of React for React components
- **Styling**: Always use Tailwind CSS for all styling needs
- **TypeScript**: Use TypeScript for type safety

### Development Workflow

1. **Plan First**: Always show your implementation plan before starting
2. **Component Structure**: 
   - Use Astro components for static content
   - Use React components for interactive features with `client:load` directive
3. **Styling**: Use Tailwind utility classes, avoid custom CSS when possible
4. **File Organization**:
   - Components go in `src/components/`
   - Pages go in `src/pages/`
   - Utilities go in `src/utils/`
   - Types go in `src/types/`

### Best Practices

- Write type-safe code with proper TypeScript interfaces
- Use functional React components with hooks
- Implement proper error handling and loading states
- Ensure responsive design for mobile and desktop
- Follow the existing component patterns (e.g., DataTable)

---

## Service Project Guidelines

### Framework and Architecture

- **Framework**: Always use .NET 10
- **Architecture**: Use Clean Architecture best practices
- **API Style**: Use Minimal API style (avoid Controllers)
- **Patterns**: 
  - Implement CQRS (Command Query Responsibility Segregation)
  - Use the project's custom mediator pattern
  - Apply Repository pattern for data access

### Code Organization

- **File Structure**: Use individual files for:
  - Classes
  - Interfaces
  - DTOs (Data Transfer Objects)
  - Enums
  - Constants
- **Layers**: Maintain clear separation between:
  - Domain Layer
  - Application Layer
  - Infrastructure Layer
  - Presentation Layer (API)

### Package Management

- **Central Package Management**: Use `ManagePackageVersionsCentrally` for all NuGet packages
- Keep package versions consistent across projects

### Testing

- **Test Framework**: Use xUnit 3 for all unit tests
- **Assertions**: Use xUnit's built-in assertions
- **Avoid**: Do NOT use FluentAssertions library
- **Coverage**: Write comprehensive unit tests for business logic

### Code Quality

- **XML Documentation**: Always add XML summary comments to:
  - Public classes
  - Public methods
  - Public properties
  - Interfaces
  - DTOs
- **OpenAPI Documentation**: Add OpenAPI/Swagger documentation for each API endpoint:
  - Use `WithName()` for operation IDs
  - Use `WithTags()` for grouping endpoints
  - Use `WithSummary()` and `WithDescription()` for documentation
  - Use `Produces()` and `ProducesValidationProblem()` for response types
  - Add example responses where applicable
- **Example**:
  ```csharp
  /// <summary>
  /// Represents a user in the system.
  /// </summary>
  public class User
  {
      /// <summary>
      /// Gets or sets the unique identifier for the user.
      /// </summary>
      public int Id { get; set; }
  }
  
  // Minimal API with OpenAPI documentation
  app.MapGet("/api/users", async (IMediator mediator, int page = 1, int pageSize = 10) =>
  {
      var query = new GetUsersQuery(page, pageSize);
      var result = await mediator.Send(query);
      return Results.Ok(result);
  })
  .WithName("GetUsers")
  .WithTags("Users")
  .WithSummary("Get paginated list of users")
  .WithDescription("Retrieves a paginated list of all users in the system")
  .Produces<PaginatedResponse<UserDto>>(StatusCodes.Status200OK)
  .ProducesValidationProblem()
  .WithOpenApi();
  ```

### Development Workflow

1. **Plan First**: Always show your implementation plan before starting
2. **CQRS**: 
   - Separate commands (write operations) from queries (read operations)
   - Use handlers for each command/query
3. **Repository Pattern**:
   - Define interfaces in Domain/Application layer
   - Implement in Infrastructure layer
4. **Mediator Pattern**:
   - Use the project's custom mediator implementation
   - Keep handlers focused and single-purpose

### Best Practices

- Follow SOLID principles
- Implement proper validation for commands
- Use async/await consistently
- Handle exceptions appropriately
- Return appropriate HTTP status codes
- Implement proper logging
- Use dependency injection
- Keep endpoint handlers focused and delegate to CQRS handlers
- Group related endpoints using route groups
- Use endpoint filters for cross-cutting concerns

---

## Cross-Cutting Concerns

### API Integration

- Client expects paginated responses with this structure:
  ```json
  {
    "data": [...],
    "total": 100,
    "page": 1,
    "pageSize": 10,
    "totalPages": 10
  }
  ```
- Service must implement CORS to allow Client requests
- Follow RESTful conventions for endpoints

### Documentation

- Update relevant documentation when making changes
- Keep API documentation in sync with implementation
- Document any breaking changes

### Git Workflow

- Work on feature branches
- Write clear commit messages
- Update CHANGELOG.md for notable changes

---

## Reminders

- ✅ Show plan before implementation
- ✅ Follow project conventions
- ✅ Write tests for new features
- ✅ Add XML comments (Service)
- ✅ Use TypeScript types (Client)
- ✅ Keep files focused and single-purpose
- ✅ Maintain Clean Architecture boundaries
