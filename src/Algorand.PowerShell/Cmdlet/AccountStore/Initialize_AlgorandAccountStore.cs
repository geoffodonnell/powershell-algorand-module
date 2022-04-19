using System;
using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.AccountStore {

	[Cmdlet(VerbsData.Initialize, "AlgorandAccountStore")]
	public class Initialize_AlgorandAccountStore : CmdletBase {

		[Parameter(Mandatory = true)]
		public string Password { get; set; }

		protected override void ProcessRecord() {

			if (PsConfiguration.AccountStore.Exists) {
				WriteError(
					new ErrorRecord(new Exception("AccountStore is already initialized, use Open-AlgorandAccountStore instead."), String.Empty, ErrorCategory.NotSpecified, this));
				return;
			}

			PsConfiguration.AccountStore.Open(Password);
		}

	}

}
