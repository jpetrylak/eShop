# eShop

A backend application that demonstrates Domain-Driven Design and CQRS using order processing in an online store. The application allows you to:

- Create an order and send a confirmation e-mail to the user
- Add a position to the order
- Remove a position from the order
- Pay for the order and send a confirmation e-mail to the user
- Ship the order and send a confirmation e-mail to the user

## Domain-Driven Design

The [Order](src/eShop.Domain/Orders/Order.cs) class exposes methods for domain operations. Each method:

- Validates incoming data and ensures the object remains in a valid state
- Updates the appropriate fields
- Adds a domain event to the `Events` collection

Domain events collected in `Events` are then processed by [DomainEventsDispatcher](src/BuildingBlocks/eShop.Shared/CQRS/DomainEventsDispatcher.cs), which:

- Invokes the domain event handler associated with the event
- Optionally publishes an integration event to RabbitMQ when one is associated with the domain event through a decorator in [ApplicationModuleExtensions](src/eShop.Application/ApplicationModuleExtensions.cs); that integration event is then handled by the integration event handler

### Domain object validation

When incoming data is invalid, the domain object throws an exception derived from [BusinessRuleException](src/BuildingBlocks/eShop.Shared/DDD/Validation/BusinessRuleException.cs). The exception is intercepted by a global error handler, [AppExceptionHandler](src/BuildingBlocks/eShop.Shared/WebApi/ErrorHandling/AppExceptionHandler.cs), in the web API layer, which returns the message to the API client.

## Solution projects

- [`eShop.WebApi`](src/eShop.WebApi) contains web controllers and application configuration.
- [`eShop.Application`](src/eShop.Application) contains queries, commands, integration events, and handlers.
- [`eShop.Domain`](src/eShop.Domain) contains domain objects with business logic, validation, and domain events.
- [`eShop.Infrastructure`](src/eShop.Infrastructure) contains Entity Framework code such as the `DbContext`, migrations, and database seeding.
- [`eShop.Shared`](src/BuildingBlocks/eShop.Shared) contains reusable building-block utilities, including code related to CQRS, DDD, error handling, and e-mail sending.
- [`FxRatesProvider`](src/FxRatesProvider) contains the foreign exchange rates worker module.

Test projects live in the root [`Tests`](Tests) folder. For example, domain unit tests are in [`Tests/eShop.Domain.UnitTests`](Tests/eShop.Domain.UnitTests), and FxRatesProvider integration tests are in [`Tests/FxRatesProvider.IntegrationTests`](Tests/FxRatesProvider.IntegrationTests).

## Libraries

- [Convey](https://github.com/snatch-dev/Convey) for CQRS and RabbitMQ messaging
- MS SQL and Entity Framework for data persistence
- [MailHog](https://github.com/mailhog/MailHog) for sending e-mails in the development environment
- xUnit and [Shouldly](https://github.com/shouldly/shouldly) for unit testing

## Docker

The repository contains a [docker-compose.yml](Scripts/docker-compose.yml) file with all required services: RabbitMQ, MS SQL, and MailHog.

## Running the application in Docker

To run the application locally using Docker:

1. Run [Generate-Certificate.ps1](Scripts/infrastructure/Generate-Certificate.ps1) to generate a self-signed certificate required for HTTPS communication.
2. Run [Build-App.ps1](Scripts/Build-App.ps1) to build the application Docker image.
3. Run [Start-App.ps1](Scripts/Start-App.ps1).

## Running the application locally

1. Install the [.NET 10 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0) and open `eShop.sln`.
2. Run or debug the `eShop.WebApi` project using the `http` launch configuration.
3. Optionally run [Start-LocalEnvironment.ps1](Scripts/infrastructure/Start-LocalEnvironment.ps1) to start containers for all required services: MS SQL, RabbitMQ, and MailHog. Otherwise, set the connection details in `appsettings.Development.json`.
