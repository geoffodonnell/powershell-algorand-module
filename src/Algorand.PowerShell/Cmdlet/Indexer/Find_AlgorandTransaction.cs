using Algorand.V2.Indexer.Model;
using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.Indexer {

	[Cmdlet(VerbsCommon.Find, "AlgorandTransaction")]
	public class Find_AlgorandTransaction : CmdletBase {

		[Parameter(
			ParameterSetName = "Lookup",
			Mandatory = true,
			ValueFromPipeline = true)]
		public string? AccountId { get; set; }

		[Parameter(
			ParameterSetName = "Search",
			Mandatory = false,
			ValueFromPipeline = false)]
		public string? Address { get; set; }

		[Parameter(
			ParameterSetName = "Search",
			Mandatory = false,
			ValueFromPipeline = false)]
		public AddressRole? AddressRole { get; set; }

		[Parameter(
			ParameterSetName = "Search",
			Mandatory = false,
			ValueFromPipeline = false)]
		public SwitchParameter ExcludeCloseTo { get; set; }

		[Parameter(
			ParameterSetName = "Search",
			Mandatory = false,
			ValueFromPipeline = false)]
		public ulong? ApplicationId { get; set; }

		[Parameter(
			Mandatory = false,
			ValueFromPipeline = false)]
		public int? AssetId { get; set; }

		[Parameter(
			Mandatory = false,
			ValueFromPipeline = false)]
		public DateTimeOffset? BeforeTime { get; set; }

		[Parameter(
			Mandatory = false,
			ValueFromPipeline = false)]
		public DateTimeOffset? AfterTime { get; set; }

		[Parameter(
			Mandatory = false,
			ValueFromPipeline = false)]
		public ulong? CurrencyGreaterThan { get; set; }

		[Parameter(
			Mandatory = false,
			ValueFromPipeline = false)]
		public ulong? CurrencyLessThan { get; set; }

		[Parameter(
			Mandatory = false,
			ValueFromPipeline = false)]
		public int? Limit { get; set; }

		[Parameter(
			Mandatory = false,
			ValueFromPipeline = false)]
		public ulong? Round { get; set; }

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
		public string? Next { get; set; }

		[Parameter(
			Mandatory = false,
			ValueFromPipeline = false)]
		public string? NotePrefix { get; set; }

		[Parameter(
			Mandatory = false,
			ValueFromPipeline = false)]
		public string? TxId { get; set; }

		[Parameter(
			Mandatory = false,
			ValueFromPipeline = false)]
		public TxType? TxType { get; set; }

		[Parameter(
			Mandatory = false,
			ValueFromPipeline = false)]
		public SigType? SigType { get; set; }

		[Parameter(
			Mandatory = false,
			ValueFromPipeline = false)]
		public SwitchParameter RekeyTo { get; set; }

		protected override void ProcessRecord() {

			object result;

			try {
				if (String.IsNullOrEmpty(AccountId)) {

					// TODO: This method has a few signatures, need to look in to how each behave.

					result = IndexerLookupApi
						.TransactionsGetAsync(
							CancellationToken, 
							AccountId, 
							Limit, 
							Next, 
							NotePrefix,
							TxType,
							SigType,
							TxId,
							Round,
							MinRound,
							MaxRound,
							AssetId,
							BeforeTime,
							AfterTime,
							CurrencyGreaterThan,
							CurrencyLessThan,
							(bool)RekeyTo)
						.GetAwaiter()
						.GetResult();
				} else {
					result = IndexerSearchApi
						.TransactionsAsync(
							CancellationToken,
							Limit,
							Next,
							NotePrefix,
							TxType,
							SigType,
							TxId,
							Round,
							MinRound,
							MaxRound,
							(ulong?)AssetId,
							BeforeTime,
							AfterTime,
							CurrencyGreaterThan,
							CurrencyLessThan, 
							Address,
							AddressRole,
							(bool)ExcludeCloseTo,
							(bool)RekeyTo,
							ApplicationId)
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
