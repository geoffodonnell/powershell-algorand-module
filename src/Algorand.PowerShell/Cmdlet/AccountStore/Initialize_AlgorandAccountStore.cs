using System;
using System.Management.Automation;
using System.Net;

namespace Algorand.PowerShell.Cmdlet.AccountStore {

	[Cmdlet(VerbsData.Initialize, "AlgorandAccountStore")]
	public class Initialize_AlgorandAccountStore : CmdletBase {

		protected override void ProcessRecord() {

			if (PsConfiguration.AccountStore.Exists) {
				WriteError(new ErrorRecord(
						new Exception("AccountStore is already initialized, use Open-AlgorandAccountStore instead."),
						String.Empty, 
						ErrorCategory.NotSpecified,
						this));
				return;
			}

			CommandRuntime.Host.UI.WriteLine("Set password for the new Account Store instance.");
			CommandRuntime.Host.UI.Write("Enter password: ");

			var cred1 = CommandRuntime.Host.UI.ReadLineAsSecureString();

			CommandRuntime.Host.UI.Write("Confirm password: ");

			var cred2 = CommandRuntime.Host.UI.ReadLineAsSecureString();

			var pw1 = (new NetworkCredential("", cred1)).Password;
			var pw2 = (new NetworkCredential("", cred2)).Password;

			if (!String.Equals(pw1, pw2, StringComparison.Ordinal)) {
				WriteError(new ErrorRecord(
					new Exception("Passwords do not match, use Initialize-AlgorandAccountStore to try again."),
					String.Empty,
					ErrorCategory.NotSpecified,
					this));
			}

			PsConfiguration.AccountStore.Open(pw1);

			CommandRuntime.Host.UI.WriteLine("");
			CommandRuntime.Host.UI.WriteLine($"Created account store: '{PsConfiguration.AccountStore.Location}'");
			CommandRuntime.Host.UI.WriteLine("");
		}

	}

}
