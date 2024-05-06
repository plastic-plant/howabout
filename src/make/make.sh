#!/usr/bin/env bash
set -euo pipefail
currentScriptDirectory="$( cd "$( dirname "${BASH_SOURCE[0]}" )" &> /dev/null && pwd )"
projectPath="$currentScriptDirectory/Make/Make.csproj"
dotnet run --project $projectPath -- "$@"