<#
.SYNOPSIS

Export payment transactions

.DESCRIPTION

Export 'Pay' and 'Axfer' type transactions sent by, or sent to, the specified account.
If no account is specified, the default account for the current network will be used.

.PARAMETER Account
Specifies the account whose transactions will be exported.

.PARAMETER Path
Specifies the name and path for the CSV-based output file. By default,
this scripts saves data to 'export.csv', and saves the file in the local directory.

.INPUTS

None. You cannot pipe objects to export-payment-transactions.ps1.

.OUTPUTS

None. export-payment-transactions.ps1 does not generate any output.

.EXAMPLE

PS> .\export-payment-transactions.ps1

.EXAMPLE

PS> .\export-payment-transactions.ps1 -Account "RWXECIK3BXIYZ3UF7EBODRP6R6UH6KBSBT6AL2PXBXTV4ZVZKL6AJUZDNI"

.EXAMPLE

PS> .\export-payment-transactions.ps1 -Account "RWXECIK3BXIYZ3UF7EBODRP6R6UH6KBSBT6AL2PXBXTV4ZVZKL6AJUZDNI" -Path ".\transactions.csv"

#>

[CmdletBinding()]
param (
    [Parameter(Position = 0, Mandatory = $false)]
    [string] $Account = $null,
    [Parameter(Position = 1, Mandatory = $false)]
    [string] $Path = "$PSScriptRoot\export.csv"
)

function Get-Columns {
    [CmdletBinding()]
    param (
        [Parameter(Position = 0, Mandatory = $true, ValueFromPipeline = $true)]
        [Algorand.Indexer.Model.Transaction] $Transaction
    )

    ## Process each value in the pipeline
    process {
        if (-not $Transaction){
            return;
        }

        $result = [ordered] @{
            ID = $Transaction.Id
            Sender = $Transaction.Sender
            Receiver = ''
            AssetId = 0
            Amount = 0
        }

        # $ALGO transfers have an AssetId = 0
        if ($Transaction.PaymentTransaction) {
            $result.Receiver = $Transaction.PaymentTransaction.Receiver
            $result.AssetId = 0
            $result.Amount = $Transaction.PaymentTransaction.Amount
        } elseif ($Transaction.AssetTransferTransaction){
            $result.Receiver = $Transaction.AssetTransferTransaction.Receiver
            $result.AssetId = $Transaction.AssetTransferTransaction.AssetId
            $result.Amount = $Transaction.AssetTransferTransaction.Amount
        }

        Write-Output $result
    }
}

function Select-Transactions {
    [CmdletBinding()]
    param (
        [Parameter(Position = 0, Mandatory = $true, ValueFromPipeline = $true)]
        [Algorand.Indexer.Model.TransactionsResponse] $Response
    )
        
    if ($Response -and $Response.Transactions) {
        $Response.Transactions | Where-Object { ($_.TxType -eq "Pay") -or ($_.TxType -eq "Axfer") } | Write-Output
    }
}

$network = Get-AlgorandNetwork

if (-not $Account){
    $Account = Get-AlgorandAccount
}

Write-Verbose "Exporting transactions for address: '$Account'"
Write-Verbose "Saving transactions to '$Path'"

# Setup query and output file parameters
$query = @{
    Address = $Account
}

$csv = @{
    Append  = $true
    Path    = $Path
}

# Overwrite existing CSV
if (Test-Path -Path $csv.Path -ErrorAction SilentlyContinue) {
    Remove-Item -Path $csv.Path -Force
}

# Get the transactions
$result = Find-AlgorandTransaction @query
$result | Select-Transactions | Get-Columns | Export-Csv @csv

while ($result.NextToken) {

    $query.Next = $result.NextToken
    $result = Find-AlgorandTransaction @query

    $result | Select-Transactions | Get-Columns | Export-Csv @csv
}

Write-Verbose "Done"