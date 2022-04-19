using Algorand.PowerShell.Model;
using System;
using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.Network {

	[Cmdlet(VerbsCommon.Remove, "AlgorandNetwork")]
	public class Remove_AlgorandNetwork : CmdletBase {

		[Parameter(Mandatory = true, ValueFromPipeline = true)]
		public NetworkModel Network { get; set; }

		protected override void ProcessRecord() {

			var value = PsConfiguration.GetNetworkOrThrow(Network.GenesisHash);

			if (String.Equals(value.GenesisHash, PsConstant.Mainnet.GenesisHash) ||
				String.Equals(value.GenesisHash, PsConstant.Testnet.GenesisHash) ||
				String.Equals(value.GenesisHash, PsConstant.Betanet.GenesisHash)) {

				WriteError(new ErrorRecord(
					new Exception($"Cannot remove this network."),
					String.Empty,
					ErrorCategory.NotSpecified,
					this));
				return;
			}

			var current = PsConfiguration.GetCurrentNetwork();

			if (String.Equals(value.GenesisHash, current.GenesisHash)) {
				WriteError(new ErrorRecord(
					new Exception($"Cannot remove the current network; switch networks and try this action again."),
					String.Empty,
					ErrorCategory.NotSpecified,
					this));
				return;
			}

			PsConfiguration.RemoveNetwork(value.GenesisHash);
		}

	}

}
