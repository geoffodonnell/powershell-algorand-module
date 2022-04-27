[CmdletBinding()]
param (
    [Parameter(Position = 0, mandatory = $false)]
    [string] $FeedUrl,
    [Parameter(Position = 1, mandatory = $false)]
    [string] $RepositoryName = "AzureArtifacts"
)

## Package source name - (arbitrary and unrelated to repository name)
$packageSourceName = "$($RepositoryName)PackageSource"

## Unregister repository
$repository = Get-PSRepository -Name $RepositoryName -ErrorAction SilentlyContinue

if ($repository) {
    Unregister-PSRepository -Name $repository.Name
    Write-Host "Repository '$($repository.Name)' removed."
} else {
    Write-Host "Repository '$RepositoryName' is not registered."
}

## Unregister package source - get it by name first, then try to find it by location
$packageSource = Get-PackageSource -Name $packageSourceName -ErrorAction SilentlyContinue

if ((-not $packageSource) -and (-not [System.String]::IsNullOrWhiteSpace($FeedUrl))) {
    $packageSource = Get-PackageSource `
        | Where { ($_.ProviderName -eq 'NuGet') -and ($_.Location -eq $FeedUrl) } `
        | Select -First 1
}

## This package source seems to be removed when the repository is removed,
## in case it's not though, try to remove it here.
if ($packageSource) {
    Unregister-PackageSource -Name $packageSource.Name
    Write-Host "Package source '$($packageSource.Name)' removed."
} else {
    Write-Host "Package source '$packageSourceName' is not registered."
}