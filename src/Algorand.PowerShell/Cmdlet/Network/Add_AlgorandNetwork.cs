using Algorand.PowerShell.Model;
using System;
using System.Linq;
using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.Network {

	[Cmdlet(VerbsCommon.Add, "AlgorandNetwork")]
	public class Add_AlgorandNetwork : CmdletBase {

		[Parameter(Mandatory = true)]
		public string Name { get; set; }

		[Parameter(Mandatory = true)]
		public string GenesisId { get; set; }

		[Parameter(Mandatory = true)]
		public BytesModel GenesisHash { get; set; }

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

			var existing = PsConfiguration.GetNetworks();

			if (existing.Any(s => String.Equals(s.Name, Name, StringComparison.OrdinalIgnoreCase))) {
				WriteError(new ErrorRecord(
					new Exception($"A network named '{Name}' already exists."),
					String.Empty,
					ErrorCategory.NotSpecified,
					this));
				return;
			}

			if (existing.Any(s => String.Equals(s.GenesisHash, GenesisHash.BytesAsBase64, StringComparison.Ordinal))) {
				WriteError(new ErrorRecord(
					new Exception($"A network with the '{GenesisHash.BytesAsBase64}' genesis hash already exists."),
					String.Empty,
					ErrorCategory.NotSpecified,
					this));
				return;
			}

			var configuration = new NetworkConfiguration {
				Name = Name,
				GenesisId = GenesisId,
				GenesisHash = GenesisHash.BytesAsBase64,
				AlgodNode = new AlgodConfiguration {
					Host = AlgodHost,
					ApiKey = AlgodApiKey,
					PrivateApiKey = AlgodPrivateApiKey
				},
				IndexerNode = new IndexerConfiguration {
					Host = IndexerHost,
					ApiKey = IndexerApiKey
				}
			};

			PsConfiguration.UpsertNetwork(configuration);
		}

	}

}
