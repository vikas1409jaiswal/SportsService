Adding Migration:
dotnet ef migrations add <migration-name> --project CricketService.Data --startup-project CricketService.Api

Updating Data base:
dotnet ef database update

Removing Migration:
dotnet ef migrations remove --project CricketService.Data --startup-project CricketService.Api