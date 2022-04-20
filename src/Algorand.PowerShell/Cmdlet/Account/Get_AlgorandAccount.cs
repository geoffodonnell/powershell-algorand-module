using Algorand.PowerShell.Model;
using System;
using System.Linq;
using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.Account {

	[Cmdlet(VerbsCommon.Get, "AlgorandAccount", DefaultParameterSetName = "Default")]
	public class Get_AlgorandAccount : CmdletBase {

		[Parameter(
			ParameterSetName = "Address",
			Mandatory = false,
			ValueFromPipeline = false)]
		public string Address { get; set; }

		[Parameter(
			ParameterSetName = "Index", 
			Mandatory = false,
			ValueFromPipeline = false)]
		public int? Index { get; set; }

		[Parameter(
			ParameterSetName = "Name",
			Mandatory = false,
			ValueFromPipeline = false)]
		public string Name { get; set; }

		[Parameter(
			ParameterSetName = "GetAll",
			Mandatory = false, 
			ValueFromPipeline = false)]
		public SwitchParameter GetAll { get; set; }

		/// <summary>
		/// This parameter is a work-around which allow for the cmdlet to be called w/o any
		/// parameters. It's value should not be used. 
		/// </summary>
		[Parameter(
			ParameterSetName = "Default",
			Mandatory = false,
			ValueFromPipeline = false,
			DontShow = true)]
		public SwitchParameter Default { get; set; }

		[Parameter(
			Mandatory = false,
			ValueFromPipeline = false)]
		public NetworkModel Network { get; set; }

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

			var network = Network != null
				? PsConfiguration.GetNetworkOrThrow(Network.GenesisHash)
				: PsConfiguration.GetCurrentNetwork();

			var accounts = PsConfiguration
				.AccountStore
				.GetAccounts(network.GenesisHash);

			if (GetAll.IsPresent && (bool)GetAll) {
				foreach (var account in accounts) {
					WriteObject(account);
				}
			} else if (!String.IsNullOrWhiteSpace(Name)) {
				var accountByName = accounts
					.FirstOrDefault(s => String.Equals(s.Name, Name, StringComparison.OrdinalIgnoreCase));

				WriteObject(accountByName);
			} else if (!String.IsNullOrWhiteSpace(Address)) {
				var accountByAddress = accounts
					.FirstOrDefault(s => String.Equals(s.Address, Address, StringComparison.Ordinal));

				WriteObject(accountByAddress);
			} else if (Index.HasValue) {
				var accountByIndex = accounts
					.Skip(Index.Value).FirstOrDefault();

				WriteObject(accountByIndex);
			} else {
				var address = PsConfiguration
					.GetDefaultAccount(network.GenesisHash);

				var accountByAddress = accounts
					.FirstOrDefault(s => String.Equals(s.Address, address, StringComparison.Ordinal));

				WriteObject(accountByAddress);
			}

		}

	}

}
