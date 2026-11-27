# Team Blazor & DevOps Standards

## C# Code Style
- Use C# file-scoped namespaces and primary constructors where appropriate.

## Architecture & Dependency Injection
- Keep business logic in services under `Services/` and inject them into `.razor` components via `@inject`.

## Testing Requirements
- Every new feature must include unit tests in `TaskTracker.Tests` using:
  - **xUnit** for services
  - **bUnit** for `.razor` components

## Documentation
- Document all new components, models, and service methods in `docs/FEATURES.md`.

## Security
- Never hardcode secrets or connection strings.
