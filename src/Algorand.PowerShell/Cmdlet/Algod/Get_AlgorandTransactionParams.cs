using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.Algod {

	[Cmdlet(VerbsCommon.Get, "AlgorandTransactionParams")]
	public class Get_AlgorandTransactionParams : CmdletBase {

		protected override void ProcessRecord() {

			try {
				var result = AlgodDefaultApi
					.ParamsAsync(CancellationToken)
					.GetAwaiter()
					.GetResult();

				WriteObject(result);
			} catch (Exception ex) {
				WriteError(new ErrorRecord(ex, String.Empty, ErrorCategory.NotSpecified, this));
			}
		}

	}

}
