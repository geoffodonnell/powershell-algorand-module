using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.Indexer {

	[Cmdlet(VerbsCommon.Find, "AlgorandLog")]
	public class Find_AlgorandLog : CmdletBase {

		[Parameter(
			Mandatory = true,
			ValueFromPipeline = true)]
		public ulong? ApplicationId { get; set; }

		[Parameter(
			Mandatory = false,
			ValueFromPipeline = false)]
		public int? Limit { get; set; }

		[Parameter(
			Mandatory = false,
			ValueFromPipeline = false)]
		public string? Next { get; set; }

		[Parameter(
			Mandatory = false,
			ValueFromPipeline = false)]
		public string? TxId { get; set; }

		[Parameter(
			Mandatory = false,
			ValueFromPipeline = false)]
		public ulong? MinRound { get; set; }

		[Parameter(
			Mandatory = false,
			ValueFromPipeline = false)]
		public ulong? MaxRound { get; set; }

		[Parameter(
			Mandatory = false,
			ValueFromPipeline = false)]
		public string? SenderAddress { get; set; }

		protected override void ProcessRecord() {

			try {
				var result = IndexerLookupApi.LogsAsync(
					CancellationToken,
					ApplicationId.GetValueOrDefault(),
					Limit,
					Next,
					TxId,
					MinRound,
					MaxRound,
					SenderAddress);

				WriteObject(result);
			} catch (Exception ex) {
				WriteError(new ErrorRecord(ex, String.Empty, ErrorCategory.NotSpecified, this));
			}
		}

	}

}
