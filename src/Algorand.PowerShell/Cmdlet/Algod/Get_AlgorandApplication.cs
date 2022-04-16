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
					.ApplicationsAsync(Id, CancellationToken)
					.GetAwaiter()
					.GetResult();

				WriteObject(result);
			} catch (Exception ex) {
				WriteError(new ErrorRecord(ex, String.Empty, ErrorCategory.NotSpecified, this));
			}
		}

	}

}
