Write-Output "Checking if EF Migrations for Pantheon.Identity exists ..."
$path = ".\src\Infrastructure\Pantheon.Infrastructure\Migrations\"

if (-not (Test-Path -Path $path)) {
    Write-Output "Creating the EF Migrations for Pantheon.Infrastructure ..."
    dotnet ef migrations add InitialCreate -p .\src\Infrastructure\Pantheon.Infrastructure\ -s .\src\Services\Hermes.API\
}
else {
    Write-Output "[Ok] EF Migrations for Pantheon.Infrastructure exists."
}

Write-Output "Checking if EF Migrations for Pantheon.Identity exists ..."
$path = ".\src\Infrastructure\Pantheon.Identity\Migrations\"

if (-not (Test-Path -Path $path)) {
    Write-Output "Creating the EF Migrations for Pantheon.Identity ..."
    dotnet ef migrations add InitialCreate -s .\src\Infrastructure\Pantheon.Identity\
}
else {
    Write-Output "[Ok] EF Migrations for Pantheon.Identity exists."
}

Write-Output "Applying EF Migrations to the Pantheon.Infrastructure database"
dotnet ef database update -p .\src\Infrastructure\Pantheon.Infrastructure\ -s .\src\Services\Hermes.API\
Write-Output "[Ok] Updated the Pantheon.Infrastructure database."

Write-Output "Applying EF Migrations to the Pantheon.Identity database"
dotnet ef database update -s .\src\Infrastructure\Pantheon.Identity\
Write-Output "[Ok] Updated the Pantheon.Identity database"