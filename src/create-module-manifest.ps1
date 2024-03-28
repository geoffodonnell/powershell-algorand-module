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

$name = Get-Item -Path $Path | Select-Object -ExpandProperty Name
$moduleName = "$($name).psd1";
$modulePath = [System.IO.Path]::GetFullPath((Join-Path -Path $Path -ChildPath $moduleName))

# Version the module based on the file version of the assembly
$rootModulePath = [System.IO.Path]::GetFullPath((Join-Path -Path $Path -ChildPath $rootModule))
$rootModuleInfo = [System.Diagnostics.FileVersionInfo]::GetVersionInfo($rootModulePath)
$version = $rootModuleInfo.FileVersion

$newModuleManifestArgs = @{
    Author                  = $author
    CmdletsToExport         = "*"
    CompanyName             = $author
    CompatiblePSEditions    = "Core"
    Description             = "Algorand Tools for PowerShell"
    Guid                    = $Guid
    LicenseUri              = "https://raw.githubusercontent.com/geoffodonnell/powershell-algorand-module/main/LICENSE"
    ModuleVersion           = $version
    Path                    = $modulePath
    PowerShellVersion       = "7.4" # Require .NET 8.0
    ProjectUri              = "https://github.com/geoffodonnell/powershell-algorand-module"
    RootModule              = $rootModule
}

# Add the prerelease string if a value was provided
if (-not [System.String]::IsNullOrWhiteSpace($Prerelease)) {
    $newModuleManifestArgs.Prerelease = $Prerelease
}

# Create the module manifest
New-ModuleManifest @newModuleManifestArgs