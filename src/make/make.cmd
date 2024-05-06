@echo off
set "CURRENTSCRIPTDIRECTORY=%~dp0"
set "PROJECTPATH=%CURRENTSCRIPTDIRECTORY%\Make\Make.csproj"
dotnet run --project %PROJECTPATH% -- %*
exit /b %ERRORLEVEL%