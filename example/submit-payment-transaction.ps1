<#
.SYNOPSIS

Submit a payment transaction

.DESCRIPTION

Submit a transaction that will transfer $ALGO from one account to another. Note, this script is rescricted to
testnet.

.PARAMETER Sender
Specifies the account to send the payment

.PARAMETER Receiver
Specifies the account to receive the payment

.PARAMETER Amount
Specifies the amount in micro-Algo to send (e.g. a value of 1000000 equals 1.0 $ALGO)

.INPUTS

None. You cannot pipe objects to export-payment-transactions.ps1.

.OUTPUTS

None. export-payment-transactions.ps1 does not generate any output.

.EXAMPLE

PS> .\submit-payment-transaction.ps1 -Receiver "RWXECIK3BXIYZ3UF7EBODRP6R6UH6KBSBT6AL2PXBXTV4ZVZKL6AJUZDNI" -Amount 1000000

.EXAMPLE

PS> .\submit-payment-transaction.ps1 -Sender "NKYFG4FQVKX3PWH6PPY2W6VMH44NMLY3E5MLCSM2Q63OVYDCTWIODBWMM4" -Receiver "RWXECIK3BXIYZ3UF7EBODRP6R6UH6KBSBT6AL2PXBXTV4ZVZKL6AJUZDNI" -Amount 1000000

#>

[CmdletBinding()]
param (
    [Parameter(Position = 0, Mandatory = $false)]
    [Algorand.Powershell.Model.AccountModel] $Sender = $null,
    [Parameter(Position = 1, Mandatory = $true)]
    [string] $Receiver,
    [Parameter(Position = 2, Mandatory = $true)]
    [ulong] $Amount = 0
)

$network = Get-AlgorandNetwork

if ($network -ne "testnet"){
    Write-Error -Message "This example is meant for testnet."
    return;
}

# If necessary, get the default account
if (-not $Sender) {
    $Sender = Get-AlgorandAccount
}

# Get the sender account information
$info = Get-AlgorandAccountInfo -Address $Sender

# Create the payment transaction
$tx = New-AlgorandPaymentTransaction -Sender $Sender -Amount $Amount -Receiver $Receiver

# Ensure the sender account has enough to send.
# This actually ignores the minimum balance required for opted-into assets
# and created assets/apps etc., it's mostly for the sake of demonstration.
if ($info.Amount -le ($Amount + $tx.fee)) {
    Write-Error -Message "Insufficent balance"
    return;
}

# Sign the transaction
$signedTx = Sign-AlgorandTransaction -Transaction $tx -Account $sender

# Submit the transaction to the network, by default this command wait for 
# confirmation that the transaction was confirmed
$result = Submit-AlgorandTransaction -Transaction $signedTx