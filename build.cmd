@echo off
echo Building SmartWizard...

echo.
echo Restoring packages...
dotnet restore

echo.
echo Building solution...
dotnet build --configuration Release --no-restore

echo.
echo Running tests...
dotnet test --configuration Release --no-build --verbosity normal

echo.
echo Packing NuGet package...
dotnet pack src\SmartWizard\SmartWizard.csproj --configuration Release --no-build --output .\artifacts

echo.
echo Build completed successfully!
pause