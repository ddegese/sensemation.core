param(
    [string]$SolutionPath = "$(Join-Path $PSScriptRoot '..\..\Sensemation.Core.sln')",
    [string]$Configuration = "Release"
)

dotnet pack $SolutionPath -c $Configuration