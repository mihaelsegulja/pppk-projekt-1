# Custom ORM

Custom ORM implementation and demonstration 

## Disclaimer

This project is an **educational ORM implementation** created as part of a university course.
It is **not intended for production use**.

The goal of this project was to explore how ORMs work internally:

- metadata discovery
- SQL generation
- basic change tracking
- migrations concept
- entity materialization

Some parts are intentionally simplified or incomplete in order to keep the scope reasonable for a semester project.

## Project Overview

This project is a **lightweight Object–Relational Mapper (ORM)** built on top of **PostgreSQL** using **.NET**.

It follows a **code-first mindset**, where entities defined in C# are mapped to database tables using reflection and metadata.

The project is split into:
- **Orm.Core** – core ORM logic (metadata, SQL builders, tracking, migrations abstractions)
- **Orm.Console** – console application used to demonstrate and test the ORM features

## Key Concepts

- **Reflection-based metadata**
  - Entity types, columns, primary keys and relationships are discovered at runtime.
- **Explicit SQL generation**
  - SQL is built manually using builders (SELECT, INSERT, UPDATE, DELETE).
- **Change tracking**
  - Tracked entities keep a snapshot of original values.
  - Updates are generated only for modified columns.
- **Explicit includes**
  - Related entities can be eagerly loaded via `Include<T>()`.
- **Manual migrations**
  - Schema changes are defined explicitly by the client.

## Features

- Entity-to-table mapping via attributes and reflection
- CRUD operations
- WHERE / ORDER BY / LIMIT / OFFSET support
- Basic change tracking (`Unchanged`, `Modified`, `Deleted`)
- Eager loading via `Include<T>()`
- Simple migration execution
- PostgreSQL support

## Known Limitations

This ORM intentionally **does not aim to replicate EF Core**.

Notable limitations include:

- Change tracking is snapshot-based and naive
- No identity map (duplicate entity instances are possible)
- Includes may cause redundant data loading
- No lazy loading
- No LINQ provider
- Limited migration automation
- No transaction batching
- No concurrency handling
- No query optimization

These tradeoffs were made to keep the project understandable and focused on core ORM concepts.

## Why Build a Custom ORM?

The purpose of this project was **learning**, not replacing existing solutions.

Building a custom ORM helps understand:
- how ORMs translate objects into SQL
- why change tracking is complex
- how migrations work under the hood
- why production ORMs are large and complex systems

## Technologies Used

- .NET
- PostgreSQL

## Setup

### Requirements

- .NET 8.0 SDK
- PostgreSQL

### Database Configuration

Create a PostgreSQL database (Docker)

```bash
docker run -d --name pppk-postgres -e POSTGRES_USER=pppk_user -e POSTGRES_PASSWORD=pppk_pass -e POSTGRES_DB=pppk_db -v pgdata:/var/lib/postgresql -p 5432:5432 postgres:18
```

Update the connection string in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "Default": "Host=localhost;Port=5432;Database=pppk_db;Username=pppk_user;Password=pppk_pass"
  }
}
```

### Run the project

```bash
dotnet run --project Orm.Console
```
