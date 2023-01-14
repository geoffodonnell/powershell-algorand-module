using Algorand.Algod.Model.Transactions;
using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.Transaction {

	[Cmdlet(VerbsCommon.New, "AlgorandApplicationDeleteTransaction")]
	public class New_AlgorandApplicationDeleteTransaction
		: NewApplicationCallTransactionCmdletBase<ApplicationDeleteTransaction> {

		[Parameter(Mandatory = true)]
		public ulong? ApplicationId { get; set; }

		protected override void ProcessRecord() {

			var result = CreateTransaction();

			result.ApplicationId = ApplicationId;

			WriteObject(result);
		}

	}

}
