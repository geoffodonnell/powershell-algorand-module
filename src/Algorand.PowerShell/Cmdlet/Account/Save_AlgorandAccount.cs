using Algorand.PowerShell.Model;
using System;
using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.Account {

	[Cmdlet(VerbsData.Save, "AlgorandAccount")]
	public class Save_AlgorandAccount : CmdletBase {

		[Parameter(
			Position = 0,
			Mandatory = true,
			ValueFromPipeline = true)]
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

			PsConfiguration.AccountStore.Add(Account);
		}

	}

}
