[CmdletBinding()]
param (
    [Parameter(Position = 0, mandatory = $true)]
    [string] $Path,
    [Parameter(Position = 1, mandatory = $true)]
    [string] $ModuleName,
    [Parameter(Position = 2, mandatory = $true)]
    [string] $FeedUrl,
    [Parameter(Position = 3, mandatory = $true)]
    [string] $UserName,
    [Parameter(Position = 4, mandatory = $true)]
    [string] $PersonalAccessToken,
    [Parameter(Position = 5, mandatory = $false)]
    [string] $RepositoryName = $null
)

$token = "$PersonalAccessToken" | ConvertTo-SecureString -AsPlainText -Force
$creds = New-Object System.Management.Automation.PSCredential($email, $token)
$modulePath = [System.IO.Path]::GetFullPath((Join-Path -Path $Path -ChildPath $ModuleName))

## Repository & Package Source names - (both names are arbitrary and unrelated)
$repositoryName = $RepositoryName ?? "AzureArtifacts"
$packageSourceName = "$($PowershellAzureDevopsServices)PackageSource"

## Force TLS1.2
[System.Net.ServicePointManager]::SecurityProtocol = [System.Net.SecurityProtocolType]::Tls12

## Register repository
if (-not (Get-PSRepository -Name $repositoryName -ErrorAction SilentlyContinue)) {
    $registerArgs = @{
        Name                = $repositoryName
        SourceLocation      = $FeedUrl
        PublishLocation     = $FeedUrl
        InstallationPolicy  = 'Trusted'
        Credential          = $creds
    }

    Register-PSRepository @registerArgs
} else {
    Write-Host "Repository named '$repositoryName' is already registered."
}

## Register package source
if (-not (Get-PackageSource -Name $packageSourceName -ErrorAction SilentlyContinue)) {

    $found = Get-PackageSource | Where { ($_.ProviderName -eq 'NuGet') -and ($_.Location -eq $FeedUrl) } | Measure-Object

    if ($found.Count -eq 0) {
        Register-PackageSource -Name $packageSourceName -Location $FeedUrl -ProviderName NuGet -SkipValidate
    } else {
        Write-Host "Package Source with Location='$FeedUrl' is already registered."
    }
} else {
    Write-Host "Package Source named '$packageSourceName' is already registered."
}

## Publish the module
Publish-Module -Path $modulePath -Repository $repositoryName -NuGetApiKey "$PersonalAccessToken"