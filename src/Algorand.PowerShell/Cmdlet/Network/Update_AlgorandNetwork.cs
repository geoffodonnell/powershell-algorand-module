using Algorand.PowerShell.Model;
using System;
using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.Network {

	[Cmdlet(VerbsData.Update, "AlgorandNetwork")]
	public class Update_AlgorandNetwork : CmdletBase {

		[Parameter(Mandatory = true, ValueFromPipeline = true)]
		public NetworkModel Network { get; set; }

		[Parameter(Mandatory = false)]
		public string AlgodHost { get; set; }

		[Parameter(Mandatory = false)]
		public string AlgodApiKey { get; set; }

		[Parameter(Mandatory = false)]
		public string AlgodPrivateApiKey { get; set; }

		[Parameter(Mandatory = false)]
		public string IndexerHost { get; set; }

		[Parameter(Mandatory = false)]
		public string IndexerApiKey { get; set; }

		protected override void ProcessRecord() {

			var configuration = PsConfiguration.GetNetworkOrThrow(Network.GenesisHash);

			if (!String.IsNullOrWhiteSpace(AlgodHost)) {
				configuration.AlgodNode.Host = AlgodHost;
			}

			// Allow an empty string to clear the existing value
			if (AlgodApiKey != null) {
				configuration.AlgodNode.ApiKey = AlgodApiKey;
			}

			// Allow an empty string to clear the existing value
			if (AlgodPrivateApiKey != null) {
				configuration.AlgodNode.PrivateApiKey = AlgodPrivateApiKey;
			}

			if (!String.IsNullOrWhiteSpace(IndexerHost)) {
				configuration.IndexerNode.Host = IndexerHost;
			}

			// Allow an empty string to clear the existing value
			if (IndexerApiKey != null) {
				configuration.IndexerNode.ApiKey = IndexerApiKey;
			}

			PsConfiguration.UpsertNetwork(configuration);

			var current = PsConfiguration.GetCurrentNetwork();

			if (String.Equals(configuration.GenesisHash, current.GenesisHash, StringComparison.Ordinal)) {
				PsEnvironment.RefreshAlgodApiServiceSettings();
				PsEnvironment.RefreshIndexerApiServiceSettings();
			}
		}

	}

}
