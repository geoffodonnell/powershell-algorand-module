using System;
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
		public ulong? Limit { get; set; }

		[Parameter(
			Mandatory = false,
			ValueFromPipeline = false)]
		public string Next { get; set; }

		[Parameter(
			Mandatory = false,
			ValueFromPipeline = false)]
		public string TxId { get; set; }

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
		public Address SenderAddress { get; set; }

		protected override void ProcessRecord() {

			try {
				var result = IndexerLookupApi
					.lookupApplicationLogsByIDAsync(
						CancellationToken,
						ApplicationId.GetValueOrDefault(),
						Limit,
						MinRound,
						MaxRound,
						Next,
						SenderAddress,
						TxId)
					.GetAwaiter()
					.GetResult();

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
