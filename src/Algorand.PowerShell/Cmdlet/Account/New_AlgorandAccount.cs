using Algorand.PowerShell.Model;
using System;
using System.Management.Automation;
using SdkAccount = Algorand.Account;

namespace Algorand.PowerShell.Cmdlet.Account {

	[Cmdlet(VerbsCommon.New, "AlgorandAccount")]
	public class New_AlgorandAccount : CmdletBase {

		[Parameter(Mandatory = false, ValueFromPipeline = true)]
		public string Name { get; set; }

		[Parameter(Mandatory = false, ValueFromPipeline = false)]
		public NetworkModel Network { get; set; }

		[Parameter(Mandatory = false, ValueFromPipeline = false)]
		public string Mnemonic { get; set; }

		protected override void ProcessRecord() {

			var name = !String.IsNullOrWhiteSpace(Name)
				? Name
				: Guid.NewGuid().ToString();

			var network = Network != null 
				? PsConfiguration.GetNetworkOrThrow(Network.GenesisHash)
				: PsConfiguration.GetCurrentNetwork();

			SdkAccount account;

			if (!String.IsNullOrEmpty(Mnemonic)) {
				account = new SdkAccount(Mnemonic);
			} else {
				account = new SdkAccount();
			}

			var model = new AccountModel(account) {
				Name = name,
				NetworkGenesisHash = network.GenesisHash
			};

			WriteObject(model);
		}

	}

}
