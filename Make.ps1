#Basic Runner for running DOcker with Private Nuget
#
# if you get an error about permission to execure you need to run the following command in an administrator powershell prompt
# Set-ExecutionPolicy Unrestricted 
#
$output="$PSScriptRoot\out"
$appname="Nib.Exercise"

If (Test-Path $output)
{
 Remove-Item $output -Force -Recurse
}

Write-Host "Building Application" -ForegroundColor Green
dotnet build --configuration Release .\$appname.sln
dotnet publish --configuration Release -o $output .\$appname\$appname.csproj
Write-Host "Testing Application" -ForegroundColor Green
dotnet test /p:CollectCoverage=true .\$appname.UnitTests\$appname.UnitTests.csproj
      
Write-Host "Building Docker Image" -ForegroundColor Green
docker build -t "$appname/v1" -f Dockerfile.local .
Write-Host "DONE" -ForegroundColor Green