using Algorand.PowerShell.Model;
using System;
using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.Indexer {

	[Cmdlet(VerbsCommon.Find, "AlgorandTransaction")]
	public class Find_AlgorandTransaction : CmdletBase {

		[Parameter(
			Mandatory = false,
			ValueFromPipeline = false)]
		public Address Address { get; set; }

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
		public ulong? AssetId { get; set; }

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
		public ulong? Limit { get; set; }

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
		public string Next { get; set; }

		[Parameter(
			Mandatory = false,
			ValueFromPipeline = false)]
		public string NotePrefix { get; set; }

		[Parameter(
			Mandatory = false,
			ValueFromPipeline = false)]
		public string TxId { get; set; }

		[Parameter(
			Mandatory = false,
			ValueFromPipeline = false)]
		public TxType? TxType { get; set; }

		[Parameter(
			Mandatory = false,
			ValueFromPipeline = false)]
		public string SigType { get; set; }

		[Parameter(
			Mandatory = false,
			ValueFromPipeline = false)]
		public SwitchParameter RekeyTo { get; set; }

		protected override void ProcessRecord() {

			object result;

			try {
				if (!String.IsNullOrEmpty(TxId)) {
					result = IndexerLookupApi
						.lookupTransactionAsync(CancellationToken, TxId)
						.GetAwaiter()
						.GetResult();
				} else if (Address != null) {
					result = IndexerLookupApi
						.lookupAccountTransactionsAsync(
							CancellationToken,
							Address.EncodeAsString(),
							AfterTime?.ToString(PsConstant.RFC3339DataTimeFormat),
							AssetId,
							BeforeTime?.ToString(PsConstant.RFC3339DataTimeFormat),
							CurrencyGreaterThan,
							CurrencyLessThan,
							Limit,
							MaxRound,
							MinRound,
							Next,
							NotePrefix,
							(bool)RekeyTo,
							Round,
							SigType,
							TxType?.ToSdkType(),
							TxId)
						.GetAwaiter()
						.GetResult();						
				} else if (AssetId.HasValue) {
					result = IndexerLookupApi
						.lookupAssetTransactionsAsync(
							CancellationToken,
							AssetId.Value,
							Address,
							AddressRole.ToSdkType(),
							AfterTime?.ToString(PsConstant.RFC3339DataTimeFormat),
							BeforeTime?.ToString(PsConstant.RFC3339DataTimeFormat),
							CurrencyGreaterThan,
							CurrencyLessThan,
							(bool)ExcludeCloseTo,
							Limit,
							MaxRound,
							MinRound,
							Next,
							NotePrefix,
							(bool)RekeyTo,
							Round, 
							SigType,
							TxType?.ToSdkType(), 
							TxId)
						.GetAwaiter()
						.GetResult();
				} else {

					result = IndexerSearchApi
						.searchForTransactionsAsync(
							CancellationToken,
							Address,
							AddressRole.ToSdkType(),
							AfterTime?.ToString(PsConstant.RFC3339DataTimeFormat),
							ApplicationId,
							AssetId,
							BeforeTime?.ToString(PsConstant.RFC3339DataTimeFormat),
							CurrencyGreaterThan,
							CurrencyLessThan,
							(bool)ExcludeCloseTo,
							Limit,
							MaxRound,
							MinRound,
							Next, NotePrefix,
							(bool)RekeyTo,
							Round,
							SigType,
							TxType?.ToSdkType(),
							TxId)
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
