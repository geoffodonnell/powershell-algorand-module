using System;
using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.Algod {

	[Cmdlet(VerbsCommon.Get, "AlgorandSwaggerJson")]
	public class Get_AlgorandSwaggerJson : CmdletBase {

		protected override void ProcessRecord() {

			try {
				var result = AlgodCommonApi
					.Swagger_jsonAsync(CancellationToken)
					.GetAwaiter()
					.GetResult();

				WriteObject(result);
			} catch (Exception ex) {
				WriteError(new ErrorRecord(ex, String.Empty, ErrorCategory.NotSpecified, this));
			}
		}

	}

}
