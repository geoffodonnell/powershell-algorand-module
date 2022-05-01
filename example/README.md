# Algorand PSModule Examples

## Prerequisites

* Installed Algorand module
* Imported the module into the current session

## Examples

### export-payment-transactions.ps1
This script retreives transactions associated with an account and exports the Payment and ASA transfer transactions to a .CSV file.

### get-account-assets.ps1
This script retreives assets that the account has opted-in to.

### opt-out-zero-balance-asas.ps1
This script opts-out of all zero balance assets.

### submit-asset-optout-transaction.ps1
This script submits a transaction to opt-out of an asset.

### submit-payment-transaction.ps1
This script submits a $ALGO payment transaction from one account to another. This script is restricted to testnet, it will require modification to use on mainnet and should be done at your own risk.