<#
.SYNOPSIS

Get list of ASAs that an account has opted-into.

.DESCRIPTION

Get list of ASAs that an account has opted-into, optionally setting a balance range

.PARAMETER Account
Specifies the account to query.

.PARAMETER MinBalance
Specifies the minimum balance the account must hold in order for the asset to be in the result set.

.PARAMETER MaxBalance
Specifies the maximum balance the account can hold in order for the asset to be in the result set.

.INPUTS

Account

.OUTPUTS

Assets

.EXAMPLE

PS> .\get-account-assets.ps1 -Account "RWXECIK3BXIYZ3UF7EBODRP6R6UH6KBSBT6AL2PXBXTV4ZVZKL6AJUZDNI"

#>

[CmdletBinding()]
param (
    [Parameter(
        Position = 0,
        Mandatory = $true,
        ValueFromPipeline = $true)]
    [string] $Account,
    [Parameter(Position = 1, Mandatory = $false)]
    [ulong] $MinBalance = [System.UInt64]::MinValue,
    [Parameter(Position = 2, Mandatory = $false)]
    [ulong] $MaxBalance = [System.UInt64]::MaxValue
)

process {

    $info = Get-AlgorandAccountInfo -Address $Account

    # Write the assets to the pipeline
    Write-Output $info 
        | Select -ExpandProperty 'Assets'
        | Where { $_.Amount -ge $MinBalance }
        | Where { $_.Amount -le $MaxBalance }
        | ForEach { Get-AlgorandAsset -Id $_.AssetId }
        | ForEach { [PSCustomObject]@{
            Id = $_.Index
            Name = $_.Params.Name
            Url = $_.Params.Url
            Creator = $_.Params.Creator
        }}
}