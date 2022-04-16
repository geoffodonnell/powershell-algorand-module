using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.Account {

	[Cmdlet(VerbsCommon.Get, "AlgorandAccountMnemonic")]
	public class Get_AlgorandAccountMnemonic : CmdletBase {

		[Parameter(
			Mandatory = true,
			ValueFromPipeline = true)]
		public Algorand.Account Account { get; set; }

		protected override void ProcessRecord() {

			WriteObject(Account?.ToMnemonic());
		}

	}

}
