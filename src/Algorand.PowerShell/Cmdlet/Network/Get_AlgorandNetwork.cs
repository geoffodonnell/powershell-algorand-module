using Algorand.PowerShell.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.Network {

	[Cmdlet(VerbsCommon.Get, "AlgorandNetwork", DefaultParameterSetName = "Default")]
	public class Get_AlgorandNetwork : CmdletBase {

		[Parameter(Mandatory = false)]
		public string Name { get; set; }

		[Parameter(Mandatory = false)]
		public BytesModel GenesisHash { get; set; }

		[Parameter(
			ParameterSetName = "GetAll",
			Mandatory = false,
			ValueFromPipeline = false)]
		public SwitchParameter GetAll { get; set; }

		/// <summary>
		/// This parameter is a work-around which allow for the cmdlet to be called w/o any
		/// parameters. It's value should not be used. 
		/// </summary>
		[Parameter(
			ParameterSetName = "Default",
			Mandatory = false,
			ValueFromPipeline = false,
			DontShow = true)]
		public SwitchParameter Default { get; set; }

		protected override void ProcessRecord() {

			var results = new List<NetworkConfiguration>();
			var networks = PsConfiguration.GetNetworks();

			if (GetAll.IsPresent) {
				results.AddRange(networks);
			} else if (!String.IsNullOrWhiteSpace(Name)) {
				results.Add(networks
					.FirstOrDefault(s => String.Equals(s.Name, Name, StringComparison.OrdinalIgnoreCase)));
			} else if (GenesisHash != null) {
				results.Add(networks
					.FirstOrDefault(s => String.Equals(s.GenesisHash, GenesisHash.BytesAsBase64, StringComparison.OrdinalIgnoreCase)));
			} else {
				results.Add(PsConfiguration.GetCurrentNetwork());
			}

			foreach (var item in results) {
				WriteObject(new NetworkModel(item));
			}
		}

	}

}
