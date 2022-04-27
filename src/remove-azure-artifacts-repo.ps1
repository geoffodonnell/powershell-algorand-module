[CmdletBinding()]
param (
    [Parameter(Position = 0, mandatory = $false)]
    [string] $FeedUrl,
    [Parameter(Position = 1, mandatory = $false)]
    [string] $RepositoryName = $null
)

## Repository & Package Source names - (both names are arbitrary and unrelated)
$repositoryName = $RepositoryName

if ([System.String]::IsNullOrWhiteSpace($repositoryName)) {
    $repositoryName = "AzureArtifacts"
}

$packageSourceName = "$($repositoryName)PackageSource"

## Register repository
$repository = Get-PSRepository -Name $repositoryName -ErrorAction SilentlyContinue

if ($repository) {
    Unregister-PSRepository -Name $repository.Name
    Write-Host "'$($repository.Name)' removed."
} else {
    Write-Host "Repository '$repositoryName' is not registered."
}

## Register package source
$packageSource = Get-PackageSource -Name $packageSourceName -ErrorAction SilentlyContinue

if ((-not $packageSource) -and (-not [System.String]::IsNullOrWhiteSpace($FeedUrl))) {
    $packageSource = Get-PackageSource `
        | Where { ($_.ProviderName -eq 'NuGet') -and ($_.Location -eq $FeedUrl) } `
        | Select -First 1
}

if ($packageSource) {
    Unregister-PackageSource -Name $packageSource.Name
    Write-Host "Package source '$($packageSource.Name)' removed."
} else {
    Write-Host "Package source '$packageSourceName' is not registered."
}