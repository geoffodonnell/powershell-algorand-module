using System;
using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.Indexer {

	[Cmdlet(VerbsCommon.Find, "AlgorandBlock")]
	public class Find_AlgorandBlock : CmdletBase {

		[Parameter(
			Mandatory = true,
			ValueFromPipeline = false)]
		public ulong? Round { get; set; }

		protected override void ProcessRecord() {

			try {
				var result = IndexerLookupApi.BlocksAsync(
					CancellationToken,
					Round.GetValueOrDefault());

				WriteObject(result);
			} catch (Exception ex) {
				WriteError(new ErrorRecord(ex, String.Empty, ErrorCategory.NotSpecified, this));
			}
		}

	}

}
