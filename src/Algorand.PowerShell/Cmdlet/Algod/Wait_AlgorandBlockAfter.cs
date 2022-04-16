using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.Algod {

	[Cmdlet(VerbsLifecycle.Wait, "AlgorandBlockAfter")]
	public class Wait_AlgorandBlockAfter : CmdletBase {

		[Parameter(Position = 0, ValueFromPipeline = true)]
		public ulong Round { get; set; }

		protected override void ProcessRecord() {

			try {
				var result = AlgodDefaultApi
					.WaitForBlockAfterAsync(Round, CancellationToken)
					.GetAwaiter()
					.GetResult();

				WriteObject(result);
			} catch (Exception ex) {
				WriteError(new ErrorRecord(ex, String.Empty, ErrorCategory.NotSpecified, this));
			}
		}

	}

}
