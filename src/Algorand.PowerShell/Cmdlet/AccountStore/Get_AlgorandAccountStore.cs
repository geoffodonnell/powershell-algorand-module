using System;
using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.AccountStore {

	[Cmdlet(VerbsCommon.Get, "AlgorandAccountStore")]
	public class Get_AlgorandAccountStore : CmdletBase {

		protected override void ProcessRecord() {

			WriteError(
				new ErrorRecord(new NotImplementedException(), String.Empty, ErrorCategory.NotSpecified, this));
		}

	}

}
