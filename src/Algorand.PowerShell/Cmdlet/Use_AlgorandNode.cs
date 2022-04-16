using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet {

	[Cmdlet(VerbsOther.Use, "AlgorandNode")]
	public class Use_AlgorandNode : CmdletBase {

		[Parameter(ParameterSetName = "Algod")]
		public SwitchParameter Algod { get; set; }

		[Parameter(ParameterSetName = "Indexer")]
		public SwitchParameter Indexer { get; set; }

		[Parameter(Mandatory = true)]
		public string? Host { get; set; }

		[Parameter(Mandatory = false)]
		public string? ApiKey { get; set; }

		[Parameter(ParameterSetName = "Algod", Mandatory = false)]
		public string? PrivateApiKey { get; set; }

		protected override void ProcessRecord() {

			if (Algod.IsPresent) {
				Environment.SetAlgodNodeConfiguration(Host, ApiKey, PrivateApiKey);
			}

			if (Indexer.IsPresent) {
				Environment.SetIndexerNodeConfiguration(Host, ApiKey);
			}
		}

	}

}
