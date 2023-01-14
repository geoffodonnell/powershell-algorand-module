using Algorand.PowerShell.Model;
using System;
using System.Linq;
using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.Indexer {

	[Cmdlet(VerbsCommon.Find, "AlgorandAccount")]
	public class Find_AlgorandAccount : CmdletBase {

		[Parameter(
			ParameterSetName = "Lookup",
			Mandatory = true,
			ValueFromPipeline = true )]
		public string AccountId { get; set; }

		[Parameter(
			ParameterSetName = "Search",
			Mandatory = false,
			ValueFromPipeline = false)]
		public ulong? AssetId { get; set; }

		[Parameter(
			ParameterSetName = "Search",
			Mandatory = false,
			ValueFromPipeline = false)]
		public ulong? Limit { get; set; }

		[Parameter(
			ParameterSetName = "Search",
			Mandatory = false,
			ValueFromPipeline = false)]
		public string Next { get; set; }

		[Parameter(
			ParameterSetName = "Search",
			Mandatory = false,
			ValueFromPipeline = false)]
		public ulong? CurrencyGreaterThan { get; set; }

		[Parameter(
			ParameterSetName = "Search",
			Mandatory = false,
			ValueFromPipeline = false)]
		public ulong? CurrencyLessThan { get; set; }

		[Parameter(
			ParameterSetName = "Search",
			Mandatory = false,
			ValueFromPipeline = false)]
		public Address AuthAddr { get; set; }

		[Parameter(
			ParameterSetName = "Search",
			Mandatory = false,
			ValueFromPipeline = false)]
		public ulong? ApplicationId { get; set; }

		[Parameter(
			Mandatory = false,
			ValueFromPipeline = false)]
		public ulong? Round { get; set; }

		[Parameter(
			Mandatory = false,
			ValueFromPipeline = false)]
		public SwitchParameter IncludeAll { get; set; }

		[Parameter(
			Mandatory = false,
			ValueFromPipeline = false)]
		public ExcludeType[] Exclude { get; set; }

		protected override void ProcessRecord() {

			object result;
			
			try {
				if (String.IsNullOrEmpty(AccountId)) {
					result = IndexerLookupApi
						.lookupAccountByIDAsync(
							CancellationToken,
							AccountId,
							String.Join(',', Exclude?.Select(s => s.ToSdkType())),
							(bool)IncludeAll,
							Round)
						.GetAwaiter()
						.GetResult();	
				} else {
					result = IndexerSearchApi
						.searchForAccountsAsync(
							CancellationToken,
							ApplicationId,
							AssetId,
							AuthAddr, 
							CurrencyGreaterThan,
							CurrencyLessThan,
							String.Join(',', Exclude?.Select(s => s.ToSdkType())),
							(bool)IncludeAll,
							Limit,
							Next,
							Round)
						.GetAwaiter()
						.GetResult();
				}

				WriteObject(result);
			} catch (ApiException ex) {
				WriteError(new ErrorRecord(
					ex.GetExceptionWithBetterMessage(), String.Empty, ErrorCategory.NotSpecified, this));
			} catch (Exception ex) {
				WriteError(new ErrorRecord(ex, String.Empty, ErrorCategory.NotSpecified, this));
			}
		}

	}

}
