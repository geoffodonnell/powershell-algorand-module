using System;
using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.Algod {

	[Cmdlet(VerbsCommon.Remove, "AlgorandCatchup")]
	public class Remove_AlgorandCatchup : CmdletBase {

		[Parameter(
			Position = 0,
			Mandatory = true, 
			ValueFromPipeline = true)]
		public string? Catchpoint { get; set; }

		protected override void ProcessRecord() {

			try {
				var result = AlgodPrivateApi
					.CatchupDeleteAsync(Catchpoint, CancellationToken)
					.GetAwaiter()
					.GetResult();

				WriteObject(result);
			} catch (Exception ex) {
				WriteError(new ErrorRecord(ex, String.Empty, ErrorCategory.NotSpecified, this));
			}
		}

	}

}
