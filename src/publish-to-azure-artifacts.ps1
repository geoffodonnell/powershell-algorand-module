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
    [string] $RepositoryName = "AzureArtifacts"
)

$token = "$PersonalAccessToken" | ConvertTo-SecureString -AsPlainText -Force
$creds = New-Object System.Management.Automation.PSCredential($UserName, $token)
$modulePath = [System.IO.Path]::GetFullPath((Join-Path -Path $Path -ChildPath $ModuleName))

## Package Source name - (arbitrary and unrelated to repository name)
$packageSourceName = "$($RepositoryName)PackageSource"

## Force TLS1.2
[System.Net.ServicePointManager]::SecurityProtocol = [System.Net.SecurityProtocolType]::Tls12

## Register repository
if (-not (Get-PSRepository -Name $RepositoryName -ErrorAction SilentlyContinue)) {
    $registerArgs = @{
        Name                = $RepositoryName
        SourceLocation      = $FeedUrl
        PublishLocation     = $FeedUrl
        InstallationPolicy  = 'Trusted'
        Credential          = $creds
    }

    Register-PSRepository @registerArgs
} else {
    Write-Host "Repository named '$RepositoryName' is already registered."
}

## Register package source
if (-not (Get-PackageSource -Name $packageSourceName -ErrorAction SilentlyContinue)) {

    $found = Get-PackageSource | Where { ($_.ProviderName -eq 'NuGet') -and ($_.Location -eq $FeedUrl) } | Measure-Object

    if ($found.Count -eq 0) {
        Register-PackageSource -Name $packageSourceName -Location $FeedUrl -ProviderName NuGet -SkipValidate
    } else {
        Write-Host "Package source with Location='$FeedUrl' is already registered."
    }
} else {
    Write-Host "Package source named '$packageSourceName' is already registered."
}

## Publish the module
Publish-Module -Path $modulePath -Repository $RepositoryName -NuGetApiKey "$PersonalAccessToken"