# Delete all migrations and remove the database
# Be careful with this script, it will delete all migrations and the database

Write-Output "--------------------------------------------"
Write-Output "Deleting all migrations and removing the database"

dotnet ef database update 0 -p ".\webapi\webapi.csproj"
dotnet ef migrations remove -p ".\webapi\webapi.csproj"

Write-Output "--------------------------------------------"