# Create script to run aspnet-codegenerator controller for all models in the Models folder

Write-Output "--------------------------------------------"
Write-Output "Creating controllers for all models"

# If --force or -f delete all controllers
if ($args[0] -eq "--force" -or $args[0] -eq "-f") {
    Write-Output "Deleting all controllers"
    Remove-Item -Path "./Controllers" -Recurse
}

# Create the Controllers folder if it doesn't exist
if (!(Test-Path "./Controllers")) {
    New-Item -ItemType Directory -Path "./Controllers"
}

# Read all the models from the Models folder
$models = Get-ChildItem -Path "./Models" -Filter *.cs -Recurse

# Loop through all the models and create a script to run the aspnet-codegenerator controller
foreach ($model in $models) {
    $model = $model.Name -replace ".cs", ""
    Write-Output "*************************************************************"
    Write-Output "Generating controller for model: $model"
    #Write-Output "Saving controller to: .\Controllers\$model Controller.cs"
    try {
        dotnet aspnet-codegenerator controller -name $model"Controller" -api -m $model -dc UniversoContext -outDir ".\Controllers" -async -p ".\webapi.csproj"
    } catch {
        Write-Output "Error generating controller for model: $model"
        # Break the loop if there is an error
        break
    }
}

Write-Output "--------------------------------------------"