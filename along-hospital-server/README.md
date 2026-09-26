# Notes

### Running Services

- Run commands in **PowerShell** or **CMD** from the project root:  
  `\along-hospital-server\`

### Requirements

Start core dependencies:

```bash
docker compose up -d redis rabbitmq mongodb
```

### Optional

If you also need SQL Server:

```bash
docker compose up -d sqlserver
```

### Run a Specific Service

For example, to start only the Auth Service:

```bash
docker compose up -d --build auth-svc user-svc
```

### Run the Entire System

To build and start all services:

```bash
docker compose up -d --build
```

### Database Migrations

Manage EF Core migrations for all services (PowerShell scripts):

```powershell
# All services
.\scripts\add-migrations.ps1
```

```powershell
# 2 Specific services
.\scripts\add-migrations.ps1 -Services @("Product", "Order")
```

```powershell
# Specific service
.\scripts\add-migrations.ps1 -Services @("Product")
```

Remove migrations (PowerShell scripts):

```powershell
# All services
.\scripts\remove-migration.ps1
```

```powershell
# 2 Specific services
.\scripts\remove-migration.ps1 -Services @("Product", "Order")
```

```powershell
# Specific service
.\scripts\remove-migration.ps1 -Services @("Product")
```

### Environment Variables

Check which environment variables each service actually uses.

- You **do not** need to add all variables.
- Refer to **AppConfiguration** in the code and the configuration templates in the **appsettings** of the API Gateway.

### Production Deployment (Docker Swarm + Portainer)

For 2-node production setup (manager + worker) using Portainer API deployment:

- Swarm stack files: `infra-stack.yml`, `app-stack.yml`, `observability-stack.yml`
- Full tutorial: `docs/docker-swarm-portainer-tutorial.md`
- CI auto-creates and auto-updates stacks (`infra`, `app`, `observability`) through Portainer API.
- Local development can continue using `docker-compose.yml` and `docker-compose.override.yml`.
