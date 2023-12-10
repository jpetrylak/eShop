# eShop

A backend application presenting the use of the Domain Driven Development and CQRS approach on the example of order processing in an online store. The application allows to:

- Create an order (and send confirmation e-mail to user)
- Add position to the order
- Remove position from the order
- Pay for the order (and send confirmation e-mail to user)
- Ship the order (and send confirmation e-mail to user)

## Domain Driven Development

The [Order](Source/eShop.Domain/Orders/Order.cs) class provides methods related to domain operations on an object. Each method:

- Validates incoming data and ensures the correctness of own state
- Updates appropriate fields
- Adds a domain event to the *Events* collection

Domain events in the *Events* collection are then processed by [DomainEventsDispatcher](Source/eShop.Shared/CQRS/DomainEventsDispatcher.cs) which:

- Calls the domain event handler associated with the event
- Optionally, if an integration event associated with a domain event is defined (as a decorator in ([ApplicationModuleExtensions](Source/eShop.Application/ApplicationModuleExtensions.cs))), that integration event is sent to the RabbitMQ queue which will then be handled by the integration event handler

### Domain object validation

In case of erroneous incoming data, the domain object throws an exception of type inheriting from [BusinessRuleException](Source/eShop.Shared/DDD/Validation/BusinessRuleException.cs). The exception is then intercepted by a global error handler ([AppExceptionHandler](Source/eShop.Shared/WebApi/ErrorHandling/AppExceptionHandler.cs)) in the web API layer which returns message to the API client.

## Solution projects

- <code>[eShop.WebApi](Source/eShop.WebApi)</code> Contains web controllers and application configuration.
- <code>[eShop.Application](Source/eShop.Application)</code> Contains query, command integration events and handlers.
- <code>[eShop.Shared](Source/eShop.Shared)</code> Utilities used by other projects. Contains code related to CQRS, DDD, error handling, e-mails sending.
- <code>[eShop.Domain](Source/eShop.Domain)</code> Contains domain objects with business logic, validation and domain events.
- <code>[eShop.Infrastructure](Source/eShop.Infrastructure)</code> Contains Entity Framework related code like DbContext class, migrations and database seeding class.

## Libraries

- [Convey](https://github.com/snatch-dev/Convey) for CQRS and RabbitMQ messaging
- MS SQL and Entity Frameowrk for data persistence
- [MailHog](https://github.com/mailhog/MailHog) for sending e-mails in the development environment
- xUnit and [Shouldly](https://github.com/shouldly/shouldly) for unit testing

## Docker

The repository contains a [docker-compose.yml](Scripts/docker-compose.yml) file with all required services (RabbitMQ, MS SQL, MailHog).
