The initial requirements were changed a little bit. To be more precise: 

1. Tasks are stored in Todo lists per user
2. Add simple approach to publish events  

## Solution structure

* `Minglesports.Tasks.Core` - core project with Domain models, operation handlers and provider interfaces
* `Minglesports.Tasks.Providers` - providers implementation
* `Minglesports.Tasks.Web` - API controllers
* `Minglesports.Tasks.BuildingBlocks` - shared classes
* `Minglesports.Tasks.Tests` - unit tests project

## Domain

Application has the following domain structure:

```
ToDoList : AggragateRoot
    - Id
    - UserId 
    - Task[]


Task: Entity
    - Id
    - Name
    - Description
    - Status (Pending | Completed)
    - DeadlineUtc
    - CreatedAtUtc
```

## Events

There is one event that is published from `ToDoList` aggregate: `TaskUpdatedEvent`. At this moment the handler of this event (`TaskUpdatedEventHandler`) does nothing and is used for demonstration purposes only.

In general event dispatch works in the following way: 
1. Aggregate root accumulates events via `PublishEvent` method
2. `EfUnitOfWork` during the commit of entities selected all the events from all aggregate roots that were attached to change tracker 
3. If database transaction has completed then events gathered in the previous step are sent via `ISendMessages` interface.

For test purposes in this solution `ISendMessages` was implemented to send messages to CQRS pipelines. But of course it can be replaced with any messaging system.

*This is not a fully "production ready" approach. I would add a custom state tracking mechanism to aggregate roots. That will give developers more control over message dispatch.*

## EF and database

In this solution SQL server was chosen as a persistence storage. Apart from that Owned Entity Types feature of EF is used to map entities to database tables.

Mapping configuration is located in `Providers` project, `TodoListEntityConfig` class.

## CQRS

Mediator messages have the following types: Commands, Queries and Notifications (for events).

Custom pipeline behavior (`LoggingBehaviour`) was added to request handling process. It is used to log request execution and measure execution time.

## User context

At this moment `UserId` is taken from a custom HTTP header `MS-User-Id`. But of course in a real application this should be replaced with a proper `Authorization` header with Bearer token or something similar.

## API

The following API endpoint are available:
1. `GET /api/tasks` - get the list of user tasks 

Sample response:
```
{
  "data": [
    {
      "id": "d3fc982c-b518-4474-9a82-f1265e6b47f0",
      "status": "Pending",
      "name": "My task",
      "description": "This is test description",
      "deadlineUtc": "2021-07-01T22:00:18.367",
      "createdAtUtc": "2021-07-01T22:01:51.1566326"
    }
  ],
  "errors": [],
  "success": true
}
```

2. `POST /api/tasks` - create a new task
3. `PUT /api/tasks/{id}` - update
3. `DELETE /api/tasks/{id}` - delete

## Unit tests

Unit tests are separated in 3 categories:
1. Domain - unit tests for domain models
2. Repositories - unit tests for providers / persistence storages. I prefer to use Sqlite EF provider here because in-memory database does not fully support reference constraints
3. OperationHandlers - unit tests for operation handlers

## Launch settings

Steps to start the application locally:
1. Edit connection string in `appsettings.{env}.json` file
2. Update database 
```
dotnet ef database update --project Minglesports.Tasks.Providers --startup-project Minglesports.Tasks.Web`  
```
3. Start `Minglesports.Tasks.Web` project

## Missing features

1. Logging (with custom properties per log entry and the propagation of correlation id)


