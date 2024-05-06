$projectPath = Join-Path -Path $PSScriptRoot -ChildPath "Make\Make.csproj"
dotnet run --project $projectPath -- $args
exit $LASTEXITCODE;