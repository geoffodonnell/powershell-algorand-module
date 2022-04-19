using System;
using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.AccountStore {

	[Cmdlet(VerbsCommon.Close, "AlgorandAccountStore")]
	public class Close_AlgorandAccountStore : CmdletBase {

		protected override void ProcessRecord() {

			if (!PsConfiguration.AccountStore.Exists) {
				WriteError(new ErrorRecord(
					new Exception("AccountStore is not initialized, use Initialize-AlgorandAccountStore instead."),
					String.Empty,
					ErrorCategory.NotSpecified, 
					this));
				return;
			}

			if (!PsConfiguration.AccountStore.Opened) {
				WriteError(new ErrorRecord(
					new Exception("AccountStore is not opened, use Open-AlgorandAccountStore and retry this action."),
					String.Empty,
					ErrorCategory.NotSpecified,
					this));
				return;
			}

			PsConfiguration.AccountStore.Close();
		}

	}

}
