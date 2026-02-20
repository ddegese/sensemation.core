param(
    [string]$SolutionPath = "$(Join-Path $PSScriptRoot '..\..\Sensemation.Core.sln')"
)

dotnet test $SolutionPath