[CmdletBinding()]
param (
    [Parameter(Position = 0, mandatory = $true)]
    [string] $Path,
    [Parameter(Position = 1, mandatory = $false)]
    [string] $Prerelease = $null,
    [Parameter(Position = 2, mandatory = $false)]
    [string] $Guid = "c66dcc66-564b-45c8-8406-bac225fcdf02"
)

if (-not (Test-Path -Path $Path -ErrorAction SilentlyContinue)){
    Write-Error -Message "Path does not exist: $Path"
    return;
}

$author = "Geoff O'Donnell"
$rootModule = "Algorand.PowerShell.dll"

$name = Get-Item -Path $Path | Select -ExpandProperty Name
$moduleName = "$($Name).psd1";
$modulePath = [System.IO.Path]::GetFullPath((Join-Path -Path $Path -ChildPath $moduleName))

# Version the module based on the file version of the assembly
$rootModulePath = [System.IO.Path]::GetFullPath((Join-Path -Path $Path -ChildPath $rootModule))
$rootModuleInfo = [System.Diagnostics.FileVersionInfo]::GetVersionInfo($rootModulePath)
$version = $rootModuleInfo.FileVersion

New-ModuleManifest -Author $author `
    -CmdletsToExport "*" `
    -CompanyName $author `
    -Description "Algorand Tools for PowerShell" `
    -Guid $Guid `
    -ModuleVersion $version `
    -Path $modulePath `
    -PowerShellVersion "7.0" `
    -Prerelease $Prerelease `
    -RootModule $rootModule