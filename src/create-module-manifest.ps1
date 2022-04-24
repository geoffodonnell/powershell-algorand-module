[CmdletBinding()]
param (
    [Parameter(Position = 0, mandatory = $true)]
    [string] $Path,
    [Parameter(Position = 2, mandatory = $false)]
    [string] $Version = "1.0.0",
    [Parameter(Position = 4, mandatory = $false)]
    [string] $Guid = "c66dcc66-564b-45c8-8406-bac225fcdf02"
)

if (-not (Test-Path -Path $Path -ErrorAction SilentlyContinue)){
    Write-Error -Message "Path does not exist: $Path"
    return;
}

$author = "Geoff O'Donnell"
$name = Get-Item -Path $Path | Select -ExpandProperty Name
$moduleName = "$($Name).psd1";
$modulePath = [System.IO.Path]::GetFullPath((Join-Path -Path $Path -ChildPath $moduleName))

New-ModuleManifest -Author $author `
    -CmdletsToExport "*" `
    -CompanyName $author `
    -Description "Algorand Tools for PowerShell" `
    -Guid $Guid `
    -ModuleVersion $Version `
    -Path $modulePath `
    -PowerShellVersion "7.0" `
    -RootModule ".\Algorand.PowerShell.dll"