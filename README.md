dotnet tool install --global dotnet-ef

dotnet ef migrations add "Init Database" --project Infrastructure --startup-project WebUI --output-dir Persistence\Migrations

dotnet ef database update --project Infrastructure --startup-project WebUI