using Algorand.Algod.Model;
using System;
using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.Algod {

	[Cmdlet(VerbsCommon.Get, "AlgorandBlock")]
	public class Get_AlgorandBlock : CmdletBase {

		[Parameter(Position = 0, ValueFromPipeline = true)]
		public ulong Round { get; set; }

		protected override void ProcessRecord() {

			try {
				var result = AlgodDefaultApi
					.GetBlockAsync(CancellationToken, Round, Format.Json)
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
