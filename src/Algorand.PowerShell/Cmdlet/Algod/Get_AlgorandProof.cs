using Algorand.Algod.Model;
using System;
using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.Algod {

	[Cmdlet(VerbsCommon.Get, "AlgorandProof")]
	public class Get_AlgorandProof : CmdletBase {

		[Parameter(Position = 0, ValueFromPipeline = true)]
		public ulong Round { get; set; }

		[Parameter(Position = 1, ValueFromPipeline = false)]
		public string TxId { get; set; }

		protected override void ProcessRecord() {

			try {
				var result = AlgodDefaultApi
					.GetTransactionProofAsync(CancellationToken, Round, TxId, Format.Json)
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
