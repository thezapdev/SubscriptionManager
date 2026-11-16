# Subscription Manager

A full-stack web application for tracking recurring subscriptions, monitoring spending, and managing upcoming payments.

This project is being developed as a portfolio project to strengthen practical skills in **.NET backend development**, software architecture, relational databases, frontend development, automated testing, and deployment workflows.

## Project Goals

The main goal is to build a maintainable, testable, and secure application while applying software engineering principles in a realistic project.

The areas I will focus on are:

- **Clean Architecture** - separating business logic from infrastructure and framework concerns.
- **SOLID principles** - writing code that is maintainable, extensible, and easy to test.
- **Object-oriented programming** - applying encapsulation, abstraction, and composition where they make sense.
- **REST API design** - building clear HTTP endpoints and using appropriate status codes and request/response contracts.
- **Database design** - modelling relational data, relationships, constraints, and migrations.
- **Authentication and authorization** - protecting user accounts and ensuring users can access only their own data.
- **Automated testing** - verifying business rules and API behaviour.
- **Containerization and CI** - making the application reproducible and automating build and test workflows.
- **Full-stack development** - connecting a React client to an ASP.NET Core API.

The emphasis is on understanding the decisions behind the implementation, not just making the application work.

## Technology Stack

### Backend

| Technology | Purpose |
|---|---|
| C# | Main programming language |
| .NET 10 (LTS) | Application platform |
| ASP.NET Core Web API | HTTP API and application entry point |
| Entity Framework Core | ORM, data access, and database migrations |
| SQL Server | Relational database |
| JWT Bearer Authentication | Authentication for the SPA client |
| FluentValidation | Request and input validation |
| Swagger / OpenAPI | API documentation and manual endpoint exploration |

### Frontend

| Technology | Purpose |
|---|---|
| React | User interface |
| TypeScript | Static typing for frontend code |
| Tailwind CSS | Utility-first styling |
| Vite | Frontend development server and build tooling |

### Testing, DevOps, and Tooling

| Technology | Purpose |
|---|---|
| xUnit | Unit and integration testing |
| Docker | Containerization |
| Docker Compose | Running the application and supporting services together |
| Git and GitHub | Version control and project hosting |
| GitHub Actions | Automated build and test workflows |

## Architecture

The backend follows a pragmatic **Clean Architecture** approach, organized into separate projects:

```text
SubscriptionManager/
├── backend/
│   ├── SubscriptionManager.API/
│   ├── SubscriptionManager.Application/
│   ├── SubscriptionManager.Domain/
│   └── SubscriptionManager.Infrastructure/
├── frontend/
│   └── subscription-manager-ui/
├── tests/
│   ├── SubscriptionManager.UnitTests/
│   └── SubscriptionManager.IntegrationTests/
├── docker-compose.yml
└── README.md
```

> The folder layout above describes the intended final structure. The repository may evolve incrementally as the project is implemented.

### Layer Responsibilities

- **API** — HTTP concerns, controllers/endpoints, middleware, dependency injection configuration, and application startup.
- **Application** — use cases, application services, DTOs, validation, and abstractions required by the application.
- **Domain** — core business entities, value objects (where useful), enums, and business rules. This layer should not depend on infrastructure or presentation frameworks.
- **Infrastructure** — Entity Framework Core, SQL Server integration, persistence implementations, and external-service integrations.

### Dependency Direction

Dependencies should point inward toward the business logic:

- Domain has no dependency on the other application layers.
- Application depends on Domain.
- Infrastructure depends on Application and Domain to implement required abstractions.
- API acts as the composition root and wires the application dependencies together.

The architecture will remain pragmatic. I will avoid adding abstractions, libraries, or patterns unless they solve a concrete problem.

## SOLID Principles

I will use the SOLID principles as design guidelines throughout the project:

1. **Single Responsibility Principle (SRP)** — a class or module should have one clear reason to change.
2. **Open/Closed Principle (OCP)** — behaviour should be extendable without unnecessary modification of stable code.
3. **Liskov Substitution Principle (LSP)** — implementations should be usable wherever their abstractions are expected without breaking correctness.
4. **Interface Segregation Principle (ISP)** — prefer focused interfaces over large interfaces that force consumers to depend on unused members.
5. **Dependency Inversion Principle (DIP)** — high-level business logic should depend on abstractions rather than concrete infrastructure details.

I will apply these principles where they improve the design rather than introduce interfaces or layers mechanically.

## Planned Features

### MVP

- User registration and login.
- Authentication and authorization.
- Create, read, update, and delete subscriptions.
- Assign subscriptions to categories.
- Support weekly, monthly, and yearly billing cycles.
- Calculate monthly and yearly subscription spending.
- Display a dashboard with an overview of subscriptions and costs.
- Ensure each user can access only their own subscription data.

### Potential Extensions

- Payment history (simulated; no real card payments).
- Upcoming payment reminders.
- Search, filtering, and sorting.
- Spending charts and additional statistics.
- Background services for scheduled tasks.
- Caching where it provides measurable value.

## Development Practices and Quality Goals

During development, I will focus on:

- Keeping business rules out of controllers and persistence code.
- Using DTOs instead of exposing database entities directly through the API.
- Validating incoming requests consistently.
- Handling errors in a predictable way.
- Using asynchronous I/O for database and network operations where appropriate.
- Writing unit tests for domain and application behaviour.
- Writing integration tests for important API and persistence workflows.
- Keeping configuration and secrets out of source control.
- Using meaningful Git commits and documenting important setup decisions.
- Automating restore, build, and test steps with GitHub Actions.

## Scope and Non-Goals

The initial version is intended to manage and track subscriptions, not to process real payments.

- No real payment-card processing.
- No storage of payment-card details.
- Payment records, if implemented, will be simulated tracking records.
- The first version will be a modular monolith, not a microservices system.
- I will prefer straightforward solutions over unnecessary architectural complexity.

## Learning Objectives

By completing this project, I aim to gain practical experience with:

- Designing and implementing a layered .NET application.
- Applying Clean Architecture and SOLID principles in real code.
- Building RESTful APIs with ASP.NET Core.
- Designing relational schemas and working with EF Core migrations.
- Implementing authentication, authorization, and user-specific data access.
- Building a typed frontend and integrating it with a backend API.
- Writing unit and integration tests.
- Running a multi-service application with Docker Compose.
- Creating a basic CI pipeline with GitHub Actions.
- Explaining architectural and technical trade-offs during a code review or technical interview.

## Project Status

**Status:** In development.

Features, architecture, and documentation may change as the project evolves. The implementation will be built incrementally, with a focus on understanding and applying each technology rather than rushing to complete the feature list.
