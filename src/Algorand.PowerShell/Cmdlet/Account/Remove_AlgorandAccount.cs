using Algorand.PowerShell.Model;
using System;
using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.Account {

	[Cmdlet(VerbsCommon.Remove, "AlgorandAccount")]
	public class Remove_AlgorandAccount : CmdletBase {

		[Parameter(Mandatory = true, ValueFromPipeline = true)]
		public AccountModel Account { get; set; }

		protected override void ProcessRecord() {

			if (!PsConfiguration.AccountStore.Exists) {
				WriteError(new ErrorRecord(
					new Exception("AccountStore is not initialized, use Initialize-AlgorandAccountStore and retry this action."),
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

			PsConfiguration.AccountStore.Remove(Account);
		}

	}

}
