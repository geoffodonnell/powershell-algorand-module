using System;
using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.Transaction {
	[Cmdlet(VerbsCommon.New, "AlgorandAssetTransferTransaction")]
	public class New_AlgorandAssetTransferTransaction : NewTransactionBase {

		protected override void ProcessRecord() {

			WriteError(
				new ErrorRecord(new NotImplementedException(), String.Empty, ErrorCategory.NotSpecified, this));
		}


	}


}
