# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [0.1.0] - 2025-12-02

### Added

**Client (Frontend)**
- User management page with paginated data tables
- Exercise library management page with categorized exercises
- Workout tracking page for daily workout logs
- Reusable DataTable component with user-controlled pagination (5, 10, 25, 50, 100 rows)
- Complete API integration layer with error handling and loading states
- Mock API service for development and testing without backend
- TypeScript type definitions for User, Exercise, Workout, and API responses
- Comprehensive documentation suite:
  - API Integration Guide
  - API Endpoints Reference
  - Getting Started Guide
  - Implementation Summary
  - Mock API Usage Guide
- Environment configuration with `.env.example` template
- 404 Not Found error page with navigation

**Service (Backend)**
- Clean Architecture project structure with layered organization
- API, Application, Domain, and Infrastructure layers
- Serilog integration for structured logging
- xUnit 3 test projects for all layers
- Result pattern for error handling
- Paged result support for API responses
- Central package management with Directory.Packages.props
- Static web content serving capability
- API endpoint testing with .http files

**Documentation & Tooling**
- GitHub Copilot instructions for consistent development practices
- Changelog update prompt for git tag-based versioning
- Biome integration for code linting and formatting
- Color palette documentation for design consistency
- Comprehensive README with setup instructions for both Client and Service

### Changed
- Updated Header component with navigation links to Users, Exercises, and Workouts pages
- Reorganized Client documentation into `docs/` folder for better organization
- Enhanced README with full-stack architecture overview and development workflow
- Improved project structure for better maintainability

### Fixed
- TypeScript type errors in table components with proper index signatures
- Biome linting configuration for unused variables and imports

## [v0.0.1] - 2025-11-27

### Initial Release

This is the initial commit of the IronAmbit project.

**Author:** Jim Kulba <jkulba@users.noreply.github.com>  
**Tagged:** 2025-11-29 by Jim Kulba <jkulba@gmail.com>

### Added

- `.gitignore` - Standard ignore patterns for the project (418 lines)
- `LICENSE` - Project license file (21 lines)
- `README.md` - Project readme documentation (2 lines)

**Total Changes:** 3 files changed, 441 insertions(+)

---

*Generated from git tag v0.0.1 (commit: 73a7f32)*

[Unreleased]: https://github.com/YenkoTools/IronAmbit/compare/v0.1.0...HEAD
[0.1.0]: https://github.com/YenkoTools/IronAmbit/compare/v0.0.1...v0.1.0
[v0.0.1]: https://github.com/YenkoTools/IronAmbit/releases/tag/v0.0.1
