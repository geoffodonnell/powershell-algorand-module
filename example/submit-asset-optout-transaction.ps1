<#
.SYNOPSIS

Submit a transaction to opt-out of an asset

.DESCRIPTION

Submit a transaction that will remove the asset holding from the sender account and reduce the account's minimum balance

.PARAMETER Account
Specifies the account which will perform the action

.PARAMETER Asset
Specifies the asset to remove

.PARAMETER Force
Specifies whether or not non-zero balances will be removed

.INPUTS

Account

.OUTPUTS

None. submit-asset-optout-transaction.ps1 does not generate any output.

.EXAMPLE

PS> .\submit-asset-optout-transaction.ps1 -Account $account -Asset $asset

.EXAMPLE

PS> .\submit-asset-optout-transaction.ps1 -Account $account -Asset $asset -Force

#>

[CmdletBinding()]
param (
    [Parameter(Position = 0, Mandatory = $true, ValueFromPipeline = $true)]
    [Algorand.Powershell.Model.AccountModel] $Account,
    [Parameter(Position = 1, Mandatory = $true)]
    [PSCustomObject] $Asset = 0,
    [Parameter(Position = 2, Mandatory = $false)]
    [switch] $Force
)

process {

    if (-not (Get-Member -InputObject $Asset -Name "Id")) {
        Write-Error -Message "Asset object is missing 'Id' property."
        return
    }

    if (-not (Get-Member -InputObject $Asset -Name "Creator")) {
        Write-Error -Message "Asset object is missing 'Creator' property."
        return
    }

    Write-Verbose "Retrieving '$($Account.Address)' account information."
    
    $info = Get-AlgorandAccountInfo -Address $Account

    $asaInfo = $info.Assets | Where { $_.AssetId -eq $Asset.Id}

    if ($asaInfo.Amount -gt 0 -and -not $Force.IsPresent) {
        Write-Error -Message "Asset '$AssetId' has a non-zero balance. Use -Force to complete this action."
        return
    }

    Write-Verbose "Creating transaction to opt-out of asset '$($Asset.Id)'."   

    $txParams = @{
        Sender = $Account
        AssetReceiver = $Account
        AssetCloseTo = $Asset.Creator
        XferAsset = $Asset.Id
        AssetAmount = 0
    }

    $tx = New-AlgorandAssetTransferTransaction @txParams

    Write-Verbose "Signing transaction."   

    $signedTx = Sign-AlgorandTransaction -Transaction $tx -Account $Account

    Write-Verbose "Submitting transaction."

    # Submit the transaction to the network, by default this command will
    # wait for the transaction to be confirmed
    $result = Submit-AlgorandTransaction -Transaction $signedTx

    Write-Verbose "Transaction submission complete; tx id = '$($result.TxId)'."
}