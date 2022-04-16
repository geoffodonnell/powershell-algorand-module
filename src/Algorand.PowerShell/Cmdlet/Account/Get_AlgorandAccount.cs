using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.Account {

	[Cmdlet(VerbsCommon.Get, "AlgorandAccount")]
	public class Get_AlgorandAccount : CmdletBase {

		[Parameter(Mandatory = false, ValueFromPipeline = true)]
		public string? Address { get; set; }

		protected override void ProcessRecord() {

			if (!Configuration.AccountStore.Exists) {
				WriteError(new ErrorRecord(
					new Exception("AccountStore is not initialized, use Initialize-AlgorandAccountStore and retry this action."),
					String.Empty,
					ErrorCategory.NotSpecified,
					this));
				return;
			}

			if (!Configuration.AccountStore.Opened) {
				WriteError(new ErrorRecord(
					new Exception("AccountStore is not opened, use Open-AlgorandAccountStore and retry this action."),
					String.Empty,
					ErrorCategory.NotSpecified,
					this));
				return;
			}

			var accounts = Configuration.AccountStore;

			if (String.IsNullOrEmpty(Address)) {
				foreach (var account in accounts) {
					WriteObject(account);
				}
			} else if (Address.StartsWith("*") && Address.EndsWith("*")) {

				var term = Address.Substring(1, Address.Length - 2);

				foreach (var account in accounts.Where(s => s.Address.ToString().Contains(term))) {
					WriteObject(account);
				}
			} else if (Address.StartsWith("*")) {

				var term = Address.Substring(1);

				foreach (var account in accounts.Where(s => s.Address.ToString().EndsWith(term))) {
					WriteObject(account);
				}
			} else if (Address.EndsWith("*")) {
				var term = Address.Substring(0, Address.Length - 1);

				foreach (var account in accounts.Where(s => s.Address.ToString().StartsWith(term))) {
					WriteObject(account);
				}
			} else {
				var account = accounts.FirstOrDefault(s => s.Address.ToString().Equals(Address));

				WriteObject(account);
			}

		}

	}

}
