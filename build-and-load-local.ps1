[CmdletBinding()]
param (
    [Parameter(Position = 0, mandatory = $false)]
    [string] $Configuration = "Debug"
)

$projectPath = Join-Path -Path $PSScriptRoot -ChildPath ".\src\Algorand.PowerShell\Algorand.PowerShell.csproj" `
    | Resolve-Path `
    | Select -ExpandProperty Path

$buildDir = [System.IO.Path]::GetFullPath((Join-Path -Path $PSScriptRoot -ChildPath ".\build\"))

if (Test-Path -Path "$buildDir" -ErrorAction SilentlyContinue) {
    Get-ChildItem -Path "$buildDir" -Recurse | Remove-Item -Recurse -Force
} else {
    New-Item -Path "$buildDir" -ItemType Directory
}

dotnet publish "$projectPath" --configuration "$Configuration" --output "$buildDir" --no-self-contained

$modulePath = Join-Path -Path $buildDir -ChildPath ".\Algorand.PowerShell.psd1" | Resolve-Path | Select -ExpandProperty Path

Import-Module $modulePath