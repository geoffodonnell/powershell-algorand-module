using Algorand.Common;
using System;
using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.TransactionGroup {

	[Cmdlet(VerbsLifecycle.Submit, "AlgorandTransactionGroup")]
	public class Submit_AlgorandTransactionGroup : CmdletBase {

		[Parameter(
			Mandatory = true,
			ValueFromPipeline = true)]
		public Algorand.Common.TransactionGroup Group { get; set; }

		[Parameter(
			Mandatory = false,
			ValueFromPipeline = false)]
		public SwitchParameter NoWait { get; set; }

		protected override void ProcessRecord() {

			if (!Group.IsSigned) {
				WriteError(new ErrorRecord(
					new InvalidOperationException($"All transactions must be signed before submission."),
					String.Empty,
					ErrorCategory.NotSpecified, 
					this));
			}

			try {

				// Wait for confirmation by default
				var wait = NoWait.IsPresent ? !(bool)NoWait : true;
				var result = AlgodDefaultApi
					.SubmitTransactionGroupAsync(Group, wait)
					.GetAwaiter()
					.GetResult();

				WriteObject(result);
			} catch (Exception ex) {
				WriteError(new ErrorRecord(ex, String.Empty, ErrorCategory.NotSpecified, this));
			}
		}

	}

}
