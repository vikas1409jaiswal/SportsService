Adding Migration:
dotnet ef migrations add <migration-name> --project CricketService.Data --startup-project CricketService.Api -c CricketServiceContext

Updating Data base:
dotnet ef database update -c CricketServiceContext

Removing Migration:
dotnet ef migrations remove --project CricketService.Data --startup-project CricketService.Api -c CricketServiceContext