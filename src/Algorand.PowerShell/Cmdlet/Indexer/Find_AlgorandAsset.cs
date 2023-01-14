using System;
using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.Indexer {

	[Cmdlet(VerbsCommon.Find, "AlgorandAsset")]
	public class Find_AlgorandAsset : CmdletBase {

		[Parameter(
			Mandatory = false,
			ValueFromPipeline = true)]
		public ulong? AssetId { get; set; }

		[Parameter(
			Mandatory = false,
			ValueFromPipeline = false)]
		public SwitchParameter IncludeAll { get; set; }

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
		public Address Creator { get; set; }

		[Parameter(
			ParameterSetName = "Search",
			Mandatory = false,
			ValueFromPipeline = false)]
		public string Name { get; set; }

		[Parameter(
			ParameterSetName = "Search",
			Mandatory = false,
			ValueFromPipeline = false)]
		public string Unit { get; set; }

		protected override void ProcessRecord() {

			object result;

			try {
				if (AssetId.HasValue &&
					!Limit.HasValue &&
					String.IsNullOrEmpty(Next)) {

					result = IndexerLookupApi
						.lookupAssetByIDAsync(
							CancellationToken,
							AssetId.Value,
							(bool)IncludeAll)
						.GetAwaiter()
						.GetResult();
				} else {
					result = IndexerSearchApi
						.searchForAssetsAsync(
							CancellationToken,
							AssetId,
							Creator?.EncodeAsString(),
							(bool)IncludeAll,
							Limit,
							Name,
							Next,
							Unit)
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
