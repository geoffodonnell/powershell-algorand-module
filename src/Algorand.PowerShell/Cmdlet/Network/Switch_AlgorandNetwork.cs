using Algorand.PowerShell.Model;
using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.Network {

	[Cmdlet(VerbsCommon.Switch, "AlgorandNetwork")]
	public class Switch_AlgorandNetwork : CmdletBase {

		[Parameter(Mandatory = true, ValueFromPipeline = true)]
		public NetworkModel Network { get; set; }

		protected override void ProcessRecord() {

			PsConfiguration.SetCurrentNetwork(Network.GenesisHash);
			PsEnvironment.RefreshAlgodApiServiceSettings();
			PsEnvironment.RefreshIndexerApiServiceSettings();
		}

	}

}
