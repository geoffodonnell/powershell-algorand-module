using Algorand.V2.Indexer.Model;
using System;
using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.Indexer {

	[Cmdlet(VerbsCommon.Find, "AlgorandApplication")]
	public class Find_AlgorandApplication : CmdletBase {

		[Parameter(
			Mandatory = false,
			ValueFromPipeline = true)]
		public ulong? ApplicationId { get; set; }

		[Parameter(
			Mandatory = false,
			ValueFromPipeline = false)]
		public SwitchParameter IncludeAll { get; set; }

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

		protected override void ProcessRecord() {

			object result;

			try {
				if (ApplicationId.HasValue &&
					!Limit.HasValue && 
					String.IsNullOrEmpty(Next)) {

					result = IndexerLookupApi
						.ApplicationsAsync(
							CancellationToken,
							ApplicationId.GetValueOrDefault(),
							(bool)IncludeAll)
						.GetAwaiter()
						.GetResult();
				} else {
					result = IndexerSearchApi
						.ApplicationsAsync(
							CancellationToken,
							ApplicationId,	
							(bool)IncludeAll,
							Limit,
							Next)
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
