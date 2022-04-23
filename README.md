# powershell-algorand-module
[![Donate Algo](https://img.shields.io/badge/Donate-ALGO-000000.svg?style=flat)](https://algoexplorer.io/address/EJMR773OGLFAJY5L2BCZKNA5PXLDJOWJK4ED4XDYTYH57CG3JMGQGI25DQ)

# Overview
This PowerShell module provides tools for the Algorand blockchain.

## Roadmap
- [ ] Initial implementation
- [ ] Publish to module repository
- [ ] Examples
- [ ] Help documentation 
- [ ] Advanced use cases

# Installation
tbd

# Getting Started
The module is pre-configured for Mainnet, Testnet, and Betanet. For each of the pre-configured networks, the module connects to nodes maintained by [AlgoNode.io](https://algonode.io/) (Thanks AlgoNode!). The current Algorand network configuration determines where requests are directed. The current network can be obtained by calling `Get-AlgorandNetwork` with no arguments. 

## Setting up the Account Store
This module can be configured to manage accounts. Accounts are persisted in the Account Store, which stores data in a [KeePass](https://keepass.info/) database using [pt.KeePassLibStd](https://github.com/panteam-net/pt.KeePassLibStd). To setup the Account Store, call `Initialize-AlgorandAccountStore` and enter and confirm a password. In subsequent sessions, use `Open-AlgorandAccountStore` to make the accounts accessible.

```PowerShell
PS C:\Users\admin> Initialize-AlgorandAccountStore
Set password for the new Account Store instance.
Enter password: ********
Confirm password: ********

Created account store: 'C:\Users\admin\AppData\Local\.algorand\accounts.kdbx'
```

It is not neccessary to use the Account Store to obtain an account object for signing transactions. An account object can be initialized at any time with the following command:

```PowerShell
PS C:\Users\admin> New-AlgorandAccount -Name "My Account" -Mnemonic "$ValidMnemonic"
```

## Getting the configured network
Call `Get-AlgorandNetwork` to get the current network

```PowerShell
PS C:\Users\admin> Get-AlgorandNetwork

Name    GenesisId    GenesisHash
----    ---------    -----------
testnet testnet-v1.0 SGO1GKSzyE7IEPItTxCByw9x8FmnrCDexi9/cOUJOiI=
```

Call `Get-AlgorandNetwork -GetAll` to get all networks
 
```PowerShell
PS C:\Users\admin> Get-AlgorandNetwork -GetAll

Name    GenesisId    GenesisHash
----    ---------    -----------
mainnet mainnet-v1.0 wGHE2Pwdvd7S12BL5FaOP20EGYesN73ktiC1qzkkit8=
testnet testnet-v1.0 SGO1GKSzyE7IEPItTxCByw9x8FmnrCDexi9/cOUJOiI=
betanet betanet-v1.0 mFgazF+2uRS1tMiL9dsj01hJGySEmPN28B/TjjvpVW0=
```

## Getting the node status
Use `Get-AlgorandNodeStatus` to get the status of the configured algod node.

```PowerShell
PS C:\Users\admin> Get-AlgorandNodeStatus

CatchupTime                 : 0
LastRound                   : 21140662
LastVersion                 : https://github.com/algorandfoundation/specs/tree/d5ac876d7ede07367dbaa26e149aa42589aac1f7
NextVersion                 : https://github.com/algorandfoundation/specs/tree/d5ac876d7ede07367dbaa26e149aa42589aac1f7
NextVersionRound            : 21140663
NextVersionSupported        : True
StoppedAtUnsupportedRound   : False
TimeSinceLastRound          : 1039953532
LastCatchpoint              :
Catchpoint                  :
CatchpointTotalAccounts     : 0
CatchpointProcessedAccounts : 0
CatchpointVerifiedAccounts  : 0
CatchpointTotalBlocks       : 0
CatchpointAcquiredBlocks    : 0
```

## Notes
tbd

# Usage

## Examples

### Send a payment transaction
```PowerShell
$sender = Get-AlgorandAccount
$receiver = "ZZ6Z5YKFYOEINYKVID4HNJCM23OWAP5UP6IRTE4YPY27VMXPDJHMVAWUAY"
$amount = 3000

$tx = New-AlgorandPaymentTransaction -Sender $sender -Amount $amount -Receiver $receiver
$signedTx = Sign-AlgorandTransaction -Transaction $tx -Account $sender
$result = Submit-AlgorandTransaction -Transaction $signedTx
```

## Helpful Commands

### List the available commands in the module
```PowerShell
Get-Module -Name Algorand.PowerShell | Select -ExpandProperty ExportedCommands | Select -ExpandProperty Values | Select -ExpandProperty Name
```

# Build
## Prerequisites
* .NET 6 SDK
* PowerShell 7.2

## Local
Clone this repository and execute `build-and-load-local.ps1` in a PowerShell window to build the module and import it into the current session.

## Pipelines
powershell-algorand-module build pipelines use the [Assembly Info Task](https://github.com/BMuuN/vsts-assemblyinfo-task) extension.

# License
powershell-algorand-module is licensed under a MIT license except for the exceptions listed below. See the LICENSE file for details.

## Exceptions
None.

# Disclaimer
Nothing in the repo constitutes professional and/or financial advice. Use this module at your own risk.