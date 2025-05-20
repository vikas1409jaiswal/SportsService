Adding Migration:
dotnet ef migrations add AddAllCricketTables --project CricketService.Data --startup-project CricketService.Api -c CricketServiceContext

Updating Data base:
dotnet ef database update --project CricketService.Migrator -c CricketServiceContext

Removing Migration:
dotnet ef migrations remove --project CricketService.Data --startup-project CricketService.Api -c CricketServiceContext