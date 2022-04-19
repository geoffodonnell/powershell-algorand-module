using Algorand.PowerShell.Model;
using System;

namespace Algorand.PowerShell {

	internal static class PsConstant {

		public readonly static ModuleConfiguration DefaultModuleConfiguration
			= new ModuleConfiguration {
				CurrentNetwork = Mainnet.GenesisHash,
				DefaultAccounts = {
					{ Mainnet.GenesisHash, String.Empty },
					{ Testnet.GenesisHash, String.Empty },
					{ Betanet.GenesisHash, String.Empty }
				},
				Networks = {
					{ Mainnet.GenesisHash, PsConstant.Mainnet},
					{ Testnet.GenesisHash, PsConstant.Testnet},
					{ Betanet.GenesisHash, PsConstant.Betanet},
				}
			};		

		public readonly static NetworkConfiguration Mainnet = new NetworkConfiguration {
			Name = "mainnet",
			GenesisId = "mainnet-v1.0",
			GenesisHash = "wGHE2Pwdvd7S12BL5FaOP20EGYesN73ktiC1qzkkit8=",
			AlgodNode = new AlgodConfiguration {
				Host = "https://mainnet-api.algonode.cloud/v2/status",
				ApiKey = String.Empty,
				PrivateApiKey = String.Empty,
			},
			IndexerNode = new IndexerConfiguration {
				Host = "https://mainnet-idx.algonode.cloud/cloud",
				ApiKey = String.Empty,
			}
		};

		public readonly static NetworkConfiguration Testnet = new NetworkConfiguration {
			Name = "testnet",
			GenesisId = "testnet-v1.0",
			GenesisHash = "SGO1GKSzyE7IEPItTxCByw9x8FmnrCDexi9/cOUJOiI=",
			AlgodNode = new AlgodConfiguration {
				Host = "https://testnet-api.algonode.cloud/v2/status",
				ApiKey = String.Empty,
				PrivateApiKey = String.Empty,
			},
			IndexerNode = new IndexerConfiguration {
				Host = "https://testnet-idx.algonode.cloud/health",
				ApiKey = String.Empty,
			}
		};

		public readonly static NetworkConfiguration Betanet = new NetworkConfiguration {
			Name = "betanet",
			GenesisId = "betanet-v1.0",
			GenesisHash = "mFgazF+2uRS1tMiL9dsj01hJGySEmPN28B/TjjvpVW0=",
			AlgodNode = new AlgodConfiguration {
				Host = "https://betanet-api.algonode.cloud/v2/status",
				ApiKey = String.Empty,
				PrivateApiKey = String.Empty,
			},
			IndexerNode = new IndexerConfiguration {
				Host = "https://betanet-idx.algonode.cloud/health",
				ApiKey = String.Empty,
			}
		};

	}

}
