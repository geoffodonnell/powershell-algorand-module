using Algorand.PowerShell.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.Network {

	[Cmdlet(VerbsCommon.Get, "AlgorandNetwork")]
	public class Get_AlgorandNetwork : CmdletBase {

		[Parameter(
			ParameterSetName = "Current",
			Mandatory = false)]
		public SwitchParameter Current { get; set; }

		[Parameter(
			ParameterSetName = "Name",
			Mandatory = false)]
		public string Name { get; set; }

		[Parameter(
			ParameterSetName = "GenesisHash",
			Mandatory = false)]
		public BytesModel GenesisHash { get; set; }

		protected override void ProcessRecord() {

			var results = new List<NetworkConfiguration>();
			var networks = PsConfiguration.GetNetworks();

			if (Current.IsPresent && (bool)Current) {
				results.Add(PsConfiguration.GetCurrentNetwork());
			} else if (!String.IsNullOrWhiteSpace(Name)) {
				results.Add(networks
					.FirstOrDefault(s => String.Equals(s.Name, Name, StringComparison.InvariantCultureIgnoreCase)));
			} else if (GenesisHash != null) {
				results.Add(networks
					.FirstOrDefault(s => String.Equals(s.GenesisHash, GenesisHash.BytesAsBase64, StringComparison.InvariantCulture)));
			} else {
				results.AddRange(networks);
			}

			foreach (var item in results) {
				WriteObject(new NetworkModel(item));
			}
		}

	}

}
