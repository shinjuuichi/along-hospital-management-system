# Along Hospital Server - AI Coding Instructions

## Architecture Overview

This is a .NET 8 microservices application with event-driven architecture. The system has ~20 services orchestrated via Docker Compose.

### Service Structure Pattern

Every microservice follows a strict three-layer architecture:

- `{Service}Svc.WebAPI/` - ASP.NET Core controllers, Program.cs, Dockerfile, Consumers
- `{Service}Svc.BLL/` - Business logic, DTOs, interfaces, DependencyInjection.cs, AutoMapper profiles
- `{Service}Svc.DAL/` - Entity Framework DbContext, entities, migrations

Example: `AuthService/AuthSvc.WebAPI`, `AuthService/AuthSvc.BLL`, `AuthService/AuthSvc.DAL`

### Key Components

- **API Gateway**: Ocelot-based gateway at port 8000. Routes defined in `ApiGateway/ocelot.Development.json` and `ocelot.Production.json`
- **Message Broker**: MassTransit + RabbitMQ for async communication. Events in `MessageBroker/Events/`, contracts in `MessageBroker/Contracts/`
- **SharedLibrary**: Cross-cutting concerns - middleware (`Middlewares/`), extensions (`Extensions/`), DTOs, enums, `AppConfiguration.cs` for config binding
- **Infrastructure**: SQL Server (main), MongoDB (chatbox), Redis (caching/blacklist), RabbitMQ (events)
- **Observability**: OpenTelemetry Collector → Loki (logs), Tempo (traces), Prometheus (metrics), Grafana dashboards in `infra/observability/`

## Developer Workflow

### Running Services

Execute from project root in PowerShell/CMD:

```bash
# Start infrastructure only
docker compose up -d sqlserver redis rabbitmq mongodb

# Run specific services (example)
docker compose up -d --build auth-svc user-svc api-gateway

# Run entire system
docker compose up -d --build
```

**Port Mapping**: Services run internally on 8080, exposed externally 8000-8021 (see `docker-compose.override.yml`)

- API Gateway: 8000
- Auth: 8001, User: 8002, Patient: 8003, Doctor: 8004, etc.
- Common services: Email 7999, Upload 7998, SMS 7997

### Configuration Strategy

- Each service reads from `appsettings.json` bound to `AppConfiguration` class (see `SharedLibrary/Commons/AppConfiguration.cs`)
- Environment variables override via `docker-compose.override.yml`
- **Only add config sections your service actually uses** - check `AppConfiguration` properties
- Reference `ApiGateway/appsettings.json` as the canonical template showing all available config sections

### Database Migrations

Each service manages its own database:

```bash
# From service's WebAPI directory
dotnet ef migrations add MigrationName -o ../AuthSvc.DAL/Migrations
dotnet ef database update
```

## Coding Patterns

### Service Bootstrapping Pattern

All `Program.cs` files follow this sequence:

```csharp
var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults(); // .NET Aspire defaults

var configuration = builder.Configuration.Get<AppConfiguration>()!;
builder.Services.AddSingleton(configuration);

// Service-specific registrations
builder.Services.AddInfrastructureServices(configuration); // From BLL DependencyInjection.cs
builder.Services.AddDefaultAPIServices(); // Registers controllers, etc.
builder.Services.AddSecurityServices(configuration); // JWT middleware
builder.Services.AddMessageBroker<AuthDbContext>(configuration, assembly: typeof(Program).Assembly);
builder.Services.AddMiddlewares();

var app = builder.Build();
await app.EnsureDatabaseCreatedAsync<AuthDbContext>(); // Auto-migration

app.UseSecurityServices();
app.UseMiddlewares();
app.UseMiddleware<JwtBlacklistMiddleware>(); // If auth required
app.MapDefaultEndpoints();
app.Run();
```

### Async Communication Pattern

**Publishing Events** (broadcast to multiple consumers):

```csharp
// In BLL or controller, inject IMessageBus
await _messageBus.PublishAsync(new CreateAuthEvent { UserId = user.Id });
```

**Request/Response** (single consumer):

```csharp
var response = await _messageBus.RequestAsync<CreateUserToAuthEvent, CreateUserToAuthContract>(createEvent);
```

**Creating Consumers** (in `{Service}Svc.WebAPI/Consumers/`):

```csharp
public class CreateUserToAuthConsumer(IUserService userService, IMapper mapper)
    : RequestConsumer<CreateUserToAuthEvent, CreateUserToAuthContract>
{
    protected override async Task<CreateUserToAuthContract> Handle(ConsumeContext<CreateUserToAuthEvent> context)
    {
        var dto = mapper.Map<CreateUserDTO>(context.Message);
        var result = await userService.CreateAsync(dto);
        return new CreateUserToAuthContract { IsSuccess = true, Id = result.Id };
    }
}
```

**Defining Events/Contracts**:

- Events inherit `BaseEvent` (has EventId, OccurredAt)
- Place in `MessageBroker/Events/{ServiceName}Events/`
- Contracts (responses) in `MessageBroker/Contracts/{ServiceName}Contracts/`

### Dependency Injection Pattern

Each BLL has `DependencyInjection.cs` with extension method:

```csharp
public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, AppConfiguration config)
{
    services.AddApplicationDbContext<MyDbContext>(config); // From SharedLibrary
    services.AddRedisDatabase(config); // If caching needed
    services.AddScoped<IMyService, MyService>();
    services.AddAutoMapper(typeof(MyMappingProfile).Assembly);
    return services;
}
```

### Database Context Pattern

All DbContexts inherit `BaseDbContext` from `SharedLibrary/Base/Data/SqlServerDb/`:

```csharp
public class AuthDbContext(DbContextOptions options) : BaseDbContext(options)
{
    public DbSet<AuthAccount> AuthAccount { get; set; }
    // BaseDbContext handles audit fields, soft delete, etc.
}
```

## Key Files Reference

- `docker-compose.yml` - Service definitions, health checks, networks
- `docker-compose.override.yml` - Dev environment variables and port mappings
- `ApiGateway/ocelot.{Environment}.json` - Route configuration, rate limiting, QoS
- `SharedLibrary/Extensions/MessageBrokerExtension.cs` - MassTransit setup with RabbitMQ
- `SharedLibrary/Middlewares/ExceptionHandlingMiddleware.cs` - Centralized error handling
- `SharedLibrary/Commons/AppConfiguration.cs` - All available config sections
- `.github/instructions/docker.instructions.md` - Docker Compose refactoring patterns

### Detailed References

For comprehensive guidance, see these instruction files:

- `.github/instructions/architecture.instructions.md` - System overview, service map, communication flows
- `.github/instructions/coding.instructions.md` - Naming conventions, response patterns, exception handling
- `.github/instructions/templates.instructions.md` - Ready-to-use code templates for all patterns
- `.github/instructions/skills.instructions.md` - 12 agent skills, 5 task plans, quality checklists
