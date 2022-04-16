using System;
using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.Account {

	[Cmdlet(VerbsCommon.New, "AlgorandAccount")]
	public class New_AlgorandAccount : CmdletBase {

		[Parameter(Mandatory = false, ValueFromPipeline = true)]
		public string Mnemonic { get; set; }

		protected override void ProcessRecord() {

			if (!String.IsNullOrEmpty(Mnemonic)) {
				WriteObject(new Algorand.Account(Mnemonic));
			}

			WriteObject(new Algorand.Account());
		}

	}

}
