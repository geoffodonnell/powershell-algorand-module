using System;
using System.Management.Automation;
using System.Net;

namespace Algorand.PowerShell.Cmdlet.AccountStore {

	[Cmdlet(VerbsCommon.Open, "AlgorandAccountStore")]
	public class Open_AlgorandAccountStore : CmdletBase {

		[Parameter(Mandatory = false)]
		public string Password { get; set; }

		protected override void ProcessRecord() {

			if (!PsConfiguration.AccountStore.Exists) {
				WriteError(
					new ErrorRecord(new Exception("AccountStore is not initialized, use Initialize-AlgorandAccountStore instead."), String.Empty, ErrorCategory.NotSpecified, this));
				return;
			}

			var password = Password;

			if (String.IsNullOrWhiteSpace(password)) {

				CommandRuntime.Host.UI.Write("Enter password: ");

				var secureString = CommandRuntime.Host.UI.ReadLineAsSecureString();
				password = (new NetworkCredential("", secureString)).Password;
			}

			PsConfiguration.AccountStore.Open(password);
		}

	}

}
