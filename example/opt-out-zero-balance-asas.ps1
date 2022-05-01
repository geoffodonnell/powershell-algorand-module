<#
.SYNOPSIS

Submit transactions to opt-out of all assets that have a zero balance

.DESCRIPTION

Submit transactions to opt-out of all assets that have a zero balance

.PARAMETER Account
Specifies the account which will perform the action

.INPUTS

Account

.OUTPUTS

None. opt-out-zero-balance-asas.ps1 does not generate any output.

.EXAMPLE

PS> .\opt-out-zero-balance-asas.ps1 -Account $account -Asset $asset

#>

[CmdletBinding()]
param (
    [Parameter(Position = 0, Mandatory = $true, ValueFromPipeline = $true)]
    [Algorand.Powershell.Model.AccountModel] $Account
)

begin {

    $actionName = "submit-asset-optout-transaction.ps1"
    $actionPath = [System.IO.Path]::GetFullPath((Join-Path -Path $PSScriptRoot -ChildPath $actionName))
}

process {

    Get-AlgorandAccountInfo -Address $Account
        | Select -ExpandProperty 'Assets'
        | Where { $_.Amount -eq 0 }
        | ForEach { Get-AlgorandAsset -Id $_.AssetId }
        | ForEach { [PSCustomObject]@{
            Id = $_.Index
            Name = $_.Params.Name
            Url = $_.Params.Url
            Creator = $_.Params.Creator
        }}
        | ForEach { 
            Invoke-Expression "$actionPath -Account `$Account -Asset `$_"
        }
}