using Algorand.V2.Indexer.Model;
using System;
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
		public int? Limit { get; set; }

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
		public string AuthAddr { get; set; }

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
						.AccountsAsync(
							CancellationToken,
							AccountId,
							Round,
							(bool)IncludeAll,
							Exclude)
						.GetAwaiter()
						.GetResult();	
				} else {
					result = IndexerSearchApi
						.AccountsAsync(
							CancellationToken,
							AssetId,
							Limit,
							Next,
							CurrencyGreaterThan,
							(bool)IncludeAll,
							CurrencyLessThan,
							AuthAddr, 
							Round,
							ApplicationId,
							Exclude)
						.GetAwaiter()
						.GetResult();
				}

				WriteObject(result);
			} catch (Exception ex) {
				WriteError(new ErrorRecord(ex, String.Empty, ErrorCategory.NotSpecified, this));
			}
		}

	}

}
