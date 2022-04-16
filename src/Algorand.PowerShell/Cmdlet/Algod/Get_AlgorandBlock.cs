using Algorand.V2.Algod.Model;
using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.Algod {

	[Cmdlet(VerbsCommon.Get, "AlgorandBlock")]
	public class Get_AlgorandBlock : CmdletBase {

		[Parameter(Position = 0, ValueFromPipeline = true)]
		public ulong Round { get; set; }

		protected override void ProcessRecord() {

			try {
				var result = AlgodDefaultApi
					.BlocksAsync(Round, Format.Json, CancellationToken)
					.GetAwaiter()
					.GetResult();

				WriteObject(result);
			} catch (Exception ex) {
				WriteError(new ErrorRecord(ex, String.Empty, ErrorCategory.NotSpecified, this));
			}
		}

	}

}
