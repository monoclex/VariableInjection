@echo off
SETLOCAL ENABLEDELAYEDEXPANSION

if not exist "packages" call :install

if not "%~1" == "" goto :collect

CALL :task "%~0" "VariableInjection"

exit /b 0

:collect

set "proj=%~1"

echo.Running tests for '%proj%'

cd "%proj%.Tests"

dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover

cd ..

echo.Collecting Results

packages\ReportGenerator.3.1.2\tools\ReportGenerator.exe ^
	-reports:%proj%.Tests\coverage.opencover.xml ^
	-targetdir:Reports\%proj%\

exit

:task
shift

echo. Starting task for "%~1"
start %~0 %~1
echo. Task started

exit /b 0

:install

nuget install ReportGenerator -Version 3.1.2 -OutputDirectory packages
nuget install OpenCover -Version 4.6.519 -OutputDirectory packages
nuget install Codecov -Version 1.0.3 -OutputDirectory packages

exit /b 0