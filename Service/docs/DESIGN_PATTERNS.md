# Design Patterns in IronAmbit Service

This document identifies and describes all design patterns used in the IronAmbit Service project, including their usage, the challenges they solve, and alternatives.

## Table of Contents

1. [Repository Pattern](#1-repository-pattern)
2. [Mediator Pattern](#2-mediator-pattern-custom-implementation)
3. [Command Query Responsibility Segregation (CQRS)](#3-command-query-responsibility-segregation-cqrs)
4. [Decorator Pattern (Pipeline Behaviors)](#4-decorator-pattern-pipeline-behaviors)
5. [Dependency Injection](#5-dependency-injection-di)
6. [Result Pattern](#6-result-pattern)
7. [Unit of Work Pattern](#7-unit-of-work-pattern)
8. [Base Class Pattern](#8-base-class-pattern-template-method-variant)
9. [Strategy Pattern](#9-strategy-pattern)
10. [Extension Method Pattern](#10-extension-method-pattern)
11. [Fluent Builder Pattern](#11-fluent-builder-pattern)
12. [Service Layer Pattern](#12-service-layer-pattern)
13. [Summary](#summary)

---

## 1. Repository Pattern

### Usage & Layer
- **Layer:** Infrastructure and Application layers
- **Implementation:** Generic `IRepository<T>` interface in Application layer, implemented by `Repository<T>` base class in Infrastructure layer. Specific repositories like `UserRepository`, `ExerciseRepository`, and `WorkoutRepository` extend this base implementation.

### Challenge Solved
Provides an abstraction over data access logic, decoupling the business logic from the data layer. This allows the application to work with a simple collection-like interface for querying and persisting domain entities without being coupled to Entity Framework Core or any specific data access technology.

### Alternatives
- Direct DbContext usage in business logic
- Data Access Objects (DAO) pattern
- Query Object pattern
- Specification pattern

### GoF Category
**Structural**

---

## 2. Mediator Pattern (Custom Implementation)

### Usage & Layer
- **Layer:** Application layer
- **Implementation:** Custom mediator implementation split into `CommandDispatcher` and `QueryDispatcher`. These dispatchers route commands/queries to their respective handlers (`ICommandHandler<,>` and `IQueryHandler<,>`).

### Challenge Solved
Reduces coupling between components by providing a central point for routing requests to handlers. Components don't need to know about each other directly; they only interact through the mediator. This simplifies adding new commands/queries and supports the implementation of pipeline behaviors for cross-cutting concerns.

### Alternatives
- MediatR library (popular third-party implementation)
- Direct handler invocation
- Event Bus pattern
- Command Bus pattern

### GoF Category
**Behavioral**

---

## 3. Command Query Responsibility Segregation (CQRS)

### Usage & Layer
- **Layer:** Application layer
- **Implementation:** Separate command handlers (write operations) and query handlers (read operations). Commands like `CreateUserCommand` modify state, while queries like `GetUsersQuery` only retrieve data.

### Challenge Solved
Separates read and write operations, allowing for different optimization strategies for each. Queries can be optimized for performance without affecting commands, and commands can focus on business rules and validation. Improves scalability and maintainability of complex business logic.

### Alternatives
- Traditional CRUD services
- Active Record pattern
- Transaction Script pattern

### GoF Category
**Not a GoF pattern** (Architectural pattern)

---

## 4. Decorator Pattern (Pipeline Behaviors)

### Usage & Layer
- **Layer:** Application layer
- **Implementation:** Pipeline behaviors (`ICommandPipelineBehavior<,>` and `IQueryPipelineBehavior<,>`) that wrap handler execution. Includes:
  - `CommandValidationBehavior`
  - `CommandPerformanceBehavior`
  - `CommandMetricsBehavior`
  - `QueryValidationBehavior`
  - `QueryPerformanceBehavior`
  - `QueryMetricsBehavior`

### Challenge Solved
Adds cross-cutting concerns (validation, performance monitoring, metrics) to handlers without modifying the handlers themselves. Behaviors are executed in a pipeline, allowing multiple decorations to be composed. This keeps handlers focused on business logic while enabling consistent application of infrastructure concerns.

### Alternatives
- Aspect-Oriented Programming (AOP)
- Middleware pattern
- Interceptor pattern
- Proxy pattern

### GoF Category
**Structural**

---

## 5. Dependency Injection (DI)

### Usage & Layer
- **Layer:** All layers (cross-cutting)
- **Implementation:** ASP.NET Core's built-in DI container is used throughout. Services are registered in `ServiceCollectionExtensions` classes in both Application and Infrastructure layers. Registration uses reflection to auto-discover handlers and validators.

### Challenge Solved
Inverts control of dependency creation, making components loosely coupled and testable. Dependencies are injected rather than created internally, allowing for easy swapping of implementations and mocking in tests. Supports the Single Responsibility Principle by removing object creation concerns from classes.

### Alternatives
- Service Locator pattern
- Factory pattern
- Manual dependency construction
- Constructor initialization

### GoF Category
**Creational** (often considered a variant of Abstract Factory)

---

## 6. Result Pattern

### Usage & Layer
- **Layer:** Domain and Application layers
- **Implementation:** `Result` and `Result<T>` classes in Domain.Common that encapsulate operation outcomes with success/failure states and errors. Used throughout handlers to return operation results without throwing exceptions for business rule violations.

### Challenge Solved
Provides a type-safe way to handle operation outcomes without relying on exceptions for flow control. Makes success and failure paths explicit in the code, improving readability and allowing callers to handle errors appropriately. Reduces the performance overhead of exception handling for expected failure cases.

### Alternatives
- Exception-based error handling
- Tuple returns (bool, error)
- Optional/Maybe monad
- Either monad

### GoF Category
**Not a GoF pattern** (Functional programming pattern)

---

## 7. Unit of Work Pattern

### Usage & Layer
- **Layer:** Infrastructure layer
- **Implementation:** Entity Framework Core's `DbContext` (`ApplicationDbContext`) serves as the Unit of Work. The `SaveChangesAsync` method coordinates persisting changes to multiple entities as a single transaction.

### Challenge Solved
Manages database transactions and ensures consistency when multiple repository operations need to be treated as a single atomic operation. Tracks changes to entities and coordinates writing changes to the database, providing transactional integrity.

### Alternatives
- Manual transaction management
- Ambient transactions
- Saga pattern (for distributed transactions)

### GoF Category
**Not a GoF pattern** (Enterprise pattern from Martin Fowler's Patterns of Enterprise Application Architecture)

---

## 8. Base Class Pattern (Template Method variant)

### Usage & Layer
- **Layer:** Domain layer
- **Implementation:** Abstract `Entity` base class that all domain entities inherit from. Provides common properties like `Id`, `CreatedAt`, and `UpdatedAt` that all entities share.

### Challenge Solved
Eliminates code duplication by centralizing common entity properties and behaviors. Ensures all entities have consistent identity and auditing capabilities. The `ApplicationDbContext.UpdateTimestamps()` method leverages this inheritance to apply timestamp updates uniformly.

### Alternatives
- Composition over inheritance
- Interfaces with extension methods
- Mixins (not directly supported in C#)
- Separate auditing tables

### GoF Category
**Behavioral** (Template Method pattern variant)

---

## 9. Strategy Pattern

### Usage & Layer
- **Layer:** Infrastructure layer
- **Implementation:** Database provider selection in `ServiceCollectionExtensions`. The system can switch between different database providers (SQLite, SQL Server) based on configuration without changing the application code.

### Challenge Solved
Allows the data access strategy to be selected at runtime based on configuration. Enables the application to work with different database providers without code changes, supporting different deployment scenarios (development vs. production).

### Alternatives
- Hardcoded provider selection
- Factory pattern
- Abstract Factory pattern
- Multiple implementations with conditional compilation

### GoF Category
**Behavioral**

---

## 10. Extension Method Pattern

### Usage & Layer
- **Layer:** All layers
- **Implementation:** Multiple extension classes like `ResultExtensions`, `ServiceCollectionExtensions`, and `LoggingExtensions` that add functionality to existing types without modifying them.

### Challenge Solved
Extends functionality of framework types (like `IServiceCollection`, `Result`) without inheritance or modification. Provides a fluent, discoverable API for configuration and conversions. Keeps related functionality organized while respecting the Open/Closed Principle.

### Alternatives
- Helper/utility classes with static methods
- Inheritance
- Wrapper classes
- Adapter pattern

### GoF Category
**Not a GoF pattern** (C#-specific feature that serves similar purposes to Decorator and Adapter patterns)

---

## 11. Fluent Builder Pattern

### Usage & Layer
- **Layer:** Infrastructure and API layers
- **Implementation:** Entity Framework Core's `ModelBuilder` for configuring entity mappings, and the fluent API for configuring services, OpenTelemetry, logging, etc. in `Program.cs`.

### Challenge Solved
Provides a readable, self-documenting way to configure complex objects. Method chaining creates a natural language-like API that's easier to understand and maintain than constructor parameters or property initializers.

### Alternatives
- Constructor parameters
- Object initializers
- Configuration objects
- Attributes/annotations

### GoF Category
**Creational** (Builder pattern)

---

## 12. Service Layer Pattern

### Usage & Layer
- **Layer:** Application layer
- **Implementation:** The entire Application layer acts as a service layer, exposing business operations through command and query handlers. The API layer delegates all business logic to this layer.

### Challenge Solved
Establishes a well-defined boundary between the presentation layer (API) and business logic. Keeps API endpoints thin and focused on HTTP concerns while business rules remain in the Application layer. Enables reuse of business logic across different presentation layers.

### Alternatives
- Business logic in controllers/endpoints
- Domain services
- Transaction scripts
- Application services

### GoF Category
**Not a GoF pattern** (Enterprise architectural pattern)

---

## Summary

The IronAmbit Service employs **12 distinct design patterns** across its architecture:

### Creational Patterns (2)
- Dependency Injection
- Fluent Builder

### Structural Patterns (2)
- Repository Pattern
- Decorator Pattern (Pipeline Behaviors)

### Behavioral Patterns (3)
- Mediator Pattern
- Strategy Pattern
- Base Class Pattern (Template Method variant)

### Architectural/Enterprise Patterns (5)
- CQRS
- Result Pattern
- Unit of Work
- Extension Method Pattern
- Service Layer Pattern

These patterns work together to create a maintainable, testable, and scalable service architecture following Clean Architecture principles with clear separation of concerns across layers.

---

## References

- **Gang of Four (GoF):** Design Patterns: Elements of Reusable Object-Oriented Software by Erich Gamma, Richard Helm, Ralph Johnson, and John Vlissides
- **Martin Fowler:** Patterns of Enterprise Application Architecture
- **Microsoft Docs:** ASP.NET Core Architecture Patterns
