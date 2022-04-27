[CmdletBinding()]
param (
    [Parameter(Position = 0, mandatory = $false)]
    [string] $Configuration = "Debug",
    [Parameter(Position = 1, mandatory = $false)]
    [string] $ModuleName = "Algorand.Local",
    [Parameter(Position = 2, mandatory = $false)]
    [string] $Prerelease = "dev"
)

function Get-FullPath {
    [CmdletBinding()]
    param (
        [Parameter(Position = 0, mandatory = $true)]
        [string] $RelativePath = "Debug"
    )
    $pathSeparator = [System.IO.Path]::DirectorySeparatorChar
    $childPath = $RelativePath -f $pathSeparator

    return [System.IO.Path]::GetFullPath((Join-Path -Path $PSScriptRoot -ChildPath $childPath))
}

$guid = '10fa8c08-9309-45dd-afe5-dbdb0d1e0def'
$projectPath = Get-FullPath -RelativePath ".{0}Algorand.PowerShell{0}Algorand.PowerShell.csproj"
$buildOutputPath = Get-FullPath -RelativePath ".{0}Algorand.PowerShell{0}bin{0}$Configuration{0}_publish_{0}$ModuleName"
$createModuleManifest = Get-FullPath -RelativePath ".{0}create-module-manifest.ps1"

## Clear out the build directory, create if it doesn't exist
if (Test-Path -Path "$buildOutputPath" -ErrorAction SilentlyContinue) {
    Get-ChildItem -Path "$buildOutputPath" -Recurse | Remove-Item -Recurse -Force
} else {
    New-Item -Path "$buildOutputPath" -ItemType Directory | Out-Null
}

## Build
dotnet publish "$projectPath" --configuration "$Configuration" --output "$buildOutputPath" --no-self-contained

## Create the module manifest
Invoke-Expression "$createModuleManifest -Path '$buildOutputPath' -Guid $guid -Prerelease '$Prerelease'"

## Import the module
$modulePath = Join-Path -Path $buildOutputPath -ChildPath "$ModuleName.psd1"

Import-Module -Name $modulePath

$network = Get-AlgorandNetwork

Write-Host -Message "Algorand: Connected to '$($network.Name)'"
Write-Host -Message ""