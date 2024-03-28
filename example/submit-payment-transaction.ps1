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

Sender

.OUTPUTS

None. submit-payment-transaction.ps1 does not generate any output.

.EXAMPLE

PS> .\submit-payment-transaction.ps1 -Receiver "RWXECIK3BXIYZ3UF7EBODRP6R6UH6KBSBT6AL2PXBXTV4ZVZKL6AJUZDNI" -Amount 1000000

.EXAMPLE

PS> .\submit-payment-transaction.ps1 -Sender "NKYFG4FQVKX3PWH6PPY2W6VMH44NMLY3E5MLCSM2Q63OVYDCTWIODBWMM4" -Receiver "RWXECIK3BXIYZ3UF7EBODRP6R6UH6KBSBT6AL2PXBXTV4ZVZKL6AJUZDNI" -Amount 1000000

#>

[CmdletBinding()]
param (
    [Parameter(Position = 0, Mandatory = $false, ValueFromPipeline = $true)]
    [Algorand.Powershell.Model.AccountModel] $Sender = $null,
    [Parameter(Position = 1, Mandatory = $true)]
    [string] $Receiver,
    [Parameter(Position = 2, Mandatory = $true)]
    [ulong] $Amount = 0
)

$network = Get-AlgorandNetwork | Select-Object -ExpandProperty "GenesisId"

if ($network -ne "testnet-v1.0") {
    Write-Error -Message "This example is meant for testnet."
    return;
}

# If necessary, get the default account
if (-not $Sender) {
    $Sender = Get-AlgorandAccount -GetAll | Select-Object -First 1
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

# Submit the transaction to the network, by default this command will
# wait for the transaction to be confirmed
$result = Submit-AlgorandTransaction -Transaction $signedTx