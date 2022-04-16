using System;
using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.Indexer {

	[Cmdlet(VerbsCommon.Find, "AlgorandBalance")]
	public class Find_AlgorandBalance : CmdletBase {

		[Parameter(
			Mandatory = true,
			ValueFromPipeline = true)]
		public ulong? AssetId { get; set; }

		[Parameter(
			Mandatory = false,
			ValueFromPipeline = false)]
		public SwitchParameter IncludeAll { get; set; }

		[Parameter(
			Mandatory = false,
			ValueFromPipeline = false)]
		public int? Limit { get; set; }

		[Parameter(
			Mandatory = false,
			ValueFromPipeline = false)]
		public string Next { get; set; }

		[Parameter(
			Mandatory = false,
			ValueFromPipeline = false)]
		public ulong? Round { get; set; }

		[Parameter(
			Mandatory = false,
			ValueFromPipeline = false)]
		public ulong? CurrencyGreaterThan { get; set; }

		[Parameter(
			Mandatory = false,
			ValueFromPipeline = false)]
		public ulong? CurrencyLessThan { get; set; }

		protected override void ProcessRecord() {

			try {
				var result = IndexerLookupApi.BalancesAsync(
					CancellationToken,
					AssetId.GetValueOrDefault(),
					(bool)IncludeAll,
					Limit,
					Next,
					Round, 
					CurrencyGreaterThan,
					CurrencyLessThan);

				WriteObject(result);
			} catch (Exception ex) {
				WriteError(new ErrorRecord(ex, String.Empty, ErrorCategory.NotSpecified, this));
			}
		}

	}

}
