[CmdletBinding()]
param ()

$network = Get-AlgorandNetwork

if ($network -neq "testnet"){
    Write-Error -Message "This example is meant for testnet."
    return;
}

## Get the default account
$sender = Get-AlgorandAccount

## Create a new account
$receiver = New-AlgorandAccount

## Small amount to send
$amount = 2000;

## Get the sender account information
$info = Get-AlgorandAccountInfo -Address $sender

## Create the payment transaction
$tx = New-AlgorandPaymentTransaction -Sender $account -Amount $amount -Receiver $receiver

## Ensure the sender account has enough to send.
## This actually ignores the minimum balance required for opted-into assets
## and created assets/apps etc., it's mostly for the sake of demonstration.
if ($info.Amount -lte ($amount + $tx.fee)) {
    Write-Error -Message "Insufficent balance"
    return;
}

$signedTx = Sign-AlgorandTransaction -Transaction $tx -Account $sender

$result = Submit-AlgorandTransaction -Transaction $signedTx