Param(
    [Parameter(Mandatory = $false, ValueFromPipeline = $false)]
    [string] $Account01Mnemonic,
    [Parameter(Mandatory = $false, ValueFromPipeline = $false)]
    [string] $Account02Mnemonic,
    [Parameter(Mandatory = $false, ValueFromPipeline = $false)]
    [string]$ModuleName = "Algorand"
)

# Test node
Invoke-WebRequest -Uri "https://testnet-api.algonode.cloud/" -Verbose

$amount = 1000
$account01 = New-AlgorandAccount -Name "test-account-01" -Network Testnet -Mnemonic $Account01Mnemonic
$account02 = New-AlgorandAccount -Name "test-account-02" -Network Testnet -Mnemonic $Account02Mnemonic

Write-Verbose -Message "account01 '$($account01.Address)'."
Write-Verbose -Message "account02 '$($account02.Address)'."

# Get the sender account information
$info = Get-AlgorandAccountInfo -Address $account01.Address

Write-Verbose -Message "account01 balance '$($info.Amount)'."

# Create the payment transaction
$tx = New-AlgorandPaymentTransaction -Sender $account01 -Amount $amount -Receiver $account02.Address

Write-Verbose -Message "tx amount '$($tx.Amount)'."
Write-Verbose -Message "tx fee '$($tx.fee)'."

# Ensure the sender account has enough to send.
# This actually ignores the minimum balance required for opted-into assets
# and created assets/apps etc., it's mostly for the sake of demonstration.
if ($info.Amount -le ($amount + $tx.fee)) {
    Write-Error -Message "Insufficent balance"
    exit 1;
}

# Sign the transaction
$signedTx = Sign-AlgorandTransaction -Transaction $tx -Account $account01

# Submit the transaction to the network, by default this command will
# wait for the transaction to be confirmed
$result = Submit-AlgorandTransaction -Transaction $signedTx

if ($null -eq $result -or [String]::IsNullOrWhiteSpace($result.Txid)) {
    Write-Error -Message "Transaction failed."
    exit 1;
}

Write-Verbose -Message "Transaction '$($result.Txid)' complete."