using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.Algod {

	[Cmdlet(VerbsCommon.Get, "AlgorandGenesis")]
	public class Get_AlgorandGenesis : CmdletBase {

		protected override void ProcessRecord() {

			try {
				var result = AlgodCommonApi
					.GenesisAsync(CancellationToken)
					.GetAwaiter()
					.GetResult();

				WriteObject(result);
			} catch (Exception ex) {
				WriteError(new ErrorRecord(ex, String.Empty, ErrorCategory.NotSpecified, this));
			}
		}

	}

}
