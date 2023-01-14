using System;
using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.Algod {

	[Cmdlet(VerbsCommon.Get, "AlgorandApplication")]
	public class Get_AlgorandApplication : CmdletBase {

		[Parameter(Position = 0, ValueFromPipeline = true)]
		public ulong Id { get; set; }

		protected override void ProcessRecord() {

			try {
				var result = AlgodDefaultApi
					.GetApplicationByIDAsync(CancellationToken, Id)
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
