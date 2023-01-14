using Algorand.Algod.Model.Transactions;
using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.Transaction {

	[Cmdlet(VerbsCommon.New, "AlgorandApplicationClearStateTransaction")]
	public class New_AlgorandApplicationClearStateTransaction
		: NewApplicationCallTransactionCmdletBase<ApplicationClearStateTransaction>  {

		[Parameter(Mandatory = true)]
		public ulong ApplicationId { get; set; }

		protected override void ProcessRecord() {

			var result = CreateTransaction();

			result.ApplicationId = ApplicationId;

			WriteObject(result);
		}

	}

}
