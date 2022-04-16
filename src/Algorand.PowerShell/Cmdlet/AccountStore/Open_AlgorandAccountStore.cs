using System;
using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.AccountStore {

	[Cmdlet(VerbsCommon.Open, "AlgorandAccountStore")]
	public class Open_AlgorandAccountStore : CmdletBase {

		[Parameter(Mandatory = true)]
		public string? Password { get; set; }

		protected override void ProcessRecord() {

			if (!Configuration.AccountStore.Exists) {
				WriteError(
					new ErrorRecord(new Exception("AccountStore is not initialized, use Initialize-AlgorandAccountStore instead."), String.Empty, ErrorCategory.NotSpecified, this));
				return;
			}

			Configuration.AccountStore.Open(Password);
		}

	}

}
