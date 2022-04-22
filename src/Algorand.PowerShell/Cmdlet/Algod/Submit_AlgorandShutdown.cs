using Algorand.V2.Algod.Model;
using System;
using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.Algod {

	[Cmdlet(VerbsLifecycle.Submit, "AlgorandShutdown")]
	public class Submit_AlgorandShutdown : CmdletBase {

		[Parameter(
			Position = 0,
			Mandatory = false,
			ValueFromPipeline = true)]
		public int? Timeout { get; set; }

		protected override void ProcessRecord() {

			try {
				var result = AlgodPrivateApi
					.ShutdownAsync(Timeout, CancellationToken)
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
