# Create first migration for database and update

Write-Output "--------------------------------------------"
Write-Output "Creating first migration for database and updating"

dotnet ef migrations add AddLookupTables -p ".\webapi\webapi.csproj"
dotnet ef database update -p ".\webapi\webapi.csproj"

Write-Output "--------------------------------------------b"