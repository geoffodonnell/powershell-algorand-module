using Algorand.PowerShell.Model;
using System;
using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.Account {

	[Cmdlet(VerbsData.Update, "AlgorandAccount")]
	public class Update_AlgorandAccount : CmdletBase {

		[Parameter(
			Position = 0,
			Mandatory = true,
			ValueFromPipeline = true)]
		public AccountModel Account { get; set; }

		[Parameter(
			ParameterSetName = "MakeDefault",
			Mandatory = false,
			ValueFromPipeline = false)]
		public SwitchParameter MakeDefault { get; set; }

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

			if (MakeDefault.IsPresent) {
				PsConfiguration.SetDefaultAccount(
					Account.NetworkGenesisHash, Account.Address);
			}

		}

	}

}
