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
				throw new NotSupportedException(
					"This version of the PowerShell module does not support private API endpoints.");
			} catch (ApiException ex) {
				WriteError(new ErrorRecord(
					ex.GetExceptionWithBetterMessage(), String.Empty, ErrorCategory.NotSpecified, this));
			} catch (Exception ex) {
				WriteError(new ErrorRecord(ex, String.Empty, ErrorCategory.NotSpecified, this));
			}
		}

	}

}
