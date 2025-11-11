# CricketService Copilot Instructions

## Architecture Overview
This is a modular .NET 6 solution with React frontend for cricket data management and analytics:
- **CricketService.Api**: ASP.NET Core API backend
- **CricketService.UI**: Standalone React frontend application (TypeScript/Create React App)
- **CricketService.Data**: Entity Framework Core + PostgreSQL data layer  
- **CricketService.Domain**: Domain models and business entities
- **CricketService.Hangfire**: Background job processing with Postgres storage
- **CricketService.Migrator/Seeder**: Console apps for DB operations
- **CricketService.Utils**: PDF, Telegram, translation utilities

## Database & EF Core Patterns
- Uses **PostgreSQL** with JSONB columns for complex data (see `CricketServiceContext.cs`)
- Custom JSON converters for nested objects: `JsonConvert.SerializeObject/DeserializeObject` in entity configurations
- Migration commands in `command.md`: use `CricketService.Migrator` project for updates, not direct EF commands
- Repository pattern: interfaces in `Data/Repositories/Interfaces/`, implementations inject `CricketServiceContext`

## Key Development Workflows
```bash
# Database setup
docker-compose up -d  # Starts Postgres on port 5440
dotnet run --project CricketService.Migrator  # Run migrations
dotnet run --project CricketService.Seeder    # Seed data

# EF Core migrations (see command.md for details)
dotnet ef migrations add AddAllCricketTables --project CricketService.Data --startup-project CricketService.Api -c CricketServiceContext
dotnet ef database update --project CricketService.Migrator -c CricketServiceContext
dotnet ef migrations remove --project CricketService.Data --startup-project CricketService.Api -c CricketServiceContext

# API development  
dotnet run --project CricketService.Api       # Starts API backend only
# Frontend: cd CricketService.UI && npm start  # Start React frontend separately

# Background jobs
# Access Hangfire dashboard at /hangfire when API is running
```

## React Frontend Integration
- **Standalone app**: Located in `CricketService.UI/` directory, separate from API
- **Proxy setup**: `setupProxy.js` routes `/cricketplayer`, `/cricketmatch`, `/cricketteam` to API
- **CORS**: API configured for `http://localhost:3000` in `Startup.cs`
- **TypeScript**: Models in `src/models/`, components organized by feature
- Material-UI, Chart.js, React Query for state management

## Hangfire Background Jobs
- Jobs defined in `HangfireController.cs` with enum `CricketJob`
- Repository pattern: `IHangfireRepository` for job implementations
- Custom attributes and tracing in `CricketService.Hangfire` project
- Schedule jobs via API: `GET /hangfire/jobs?jobName=UpdatePlayersCareerStatisticsJob`

## Configuration Patterns
- **Connection strings**: In `appsettings.Development.json`, section `"PostgresServer"`
- **Options pattern**: `AddOptions<T>().Bind(Configuration.GetSection())` in `Startup.cs`  
- **DI registration**: All repositories use `AddScoped<I, Implementation>` pattern

## Code Conventions
- Controllers follow RESTful naming: `CricketPlayerController`, `CricketTeamController`
- DTOs end with `DTO` suffix, responses with `Response` 
- AutoMapper profiles in `CricketService.Data.Mappings`
- Custom middleware: `ResultModifier` adds headers, registered via extension methods
- StyleCop analyzers enabled via `Directory.Build.props`

## Testing & Quality
- Quality tests in `CricketService.Data.QualityTests` project
- Frontend tests: `cd CricketService.UI && npm test`
- Use `dotnet test` for .NET projects

## Common Integration Points
- **PDF handling**: `CricketService.Utils.PDFToHtml` 
- **External cricket data**: Extensive use of ESPN Cricinfo models in `CricketService.UI/src/models/espn-cricinfo-models/`
- **File storage**: JSON files in `Data/StaticData/` for seeding
- **Custom error handling**: `ModelStateExceptionFilter` and `ValidationErrorHandler`