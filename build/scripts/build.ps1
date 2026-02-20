param(
    [string]$SolutionPath = "$(Join-Path $PSScriptRoot '..\..\Sensemation.Core.sln')"
)

dotnet build $SolutionPath