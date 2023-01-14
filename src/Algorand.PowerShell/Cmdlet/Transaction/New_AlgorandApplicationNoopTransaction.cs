using Algorand.Algod.Model.Transactions;
using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.Transaction {

	[Cmdlet(VerbsCommon.New, "AlgorandApplicationNoopTransaction")]
	public class New_AlgorandApplicationNoopTransaction
		: NewApplicationNoopTransactionCmdletBase<ApplicationNoopTransaction> {

		protected override void ProcessRecord() {

			var result = CreateTransaction();

			WriteObject(result);
		}

	}

}
