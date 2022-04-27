[CmdletBinding()]
param (
    [Parameter(Position = 0, mandatory = $true)]
    [string] $FeedUrl,
    [Parameter(Position = 1, mandatory = $true)]
    [string] $UserName,
    [Parameter(Position = 2, mandatory = $true)]
    [string] $PersonalAccessToken,
    [Parameter(Position = 3, mandatory = $false)]
    [string] $RepositoryName = "AzureArtifacts"
)

$token = "$PersonalAccessToken" | ConvertTo-SecureString -AsPlainText -Force
$creds = New-Object System.Management.Automation.PSCredential($UserName, $token)

## Package Source name - (arbitrary and unrelated to repository name)
$packageSourceName = "$($RepositoryName)PackageSource"

## Force TLS1.2
[System.Net.ServicePointManager]::SecurityProtocol = [System.Net.SecurityProtocolType]::Tls12

## Register repository
if (-not (Get-PSRepository -Name $RepositoryName -ErrorAction SilentlyContinue)) {
    $registerArgs = @{
        Name                        = $RepositoryName
        SourceLocation              = $FeedUrl
        PublishLocation             = $FeedUrl
        InstallationPolicy          = 'Trusted'
        Credential                  = $creds
    }

    Register-PSRepository @registerArgs
} else {
    Write-Host "Repository '$RepositoryName' is already registered."
}

## Register package source
if (-not (Get-PackageSource -Name $packageSourceName -ErrorAction SilentlyContinue)) {

    $found = Get-PackageSource `
        | Where { ($_.ProviderName -eq 'NuGet') -and ($_.Location -eq $FeedUrl) } `
        | Select -First 1

    if (-not $found) {
        Register-PackageSource -Name $packageSourceName -Location $FeedUrl -ProviderName NuGet -SkipValidate
    } else {
        Write-Host "Package source $($found.Name) ($($found.Location)) is already registered."
    }
} else {
    Write-Host "Package source named '$packageSourceName' is already registered."
}