using Algorand.Algod.Model.Transactions;
using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.Transaction {

	[Cmdlet(VerbsCommon.New, "AlgorandKeyRegisterOfflineTransaction")]
	public class New_AlgorandKeyRegisterOfflineTransaction :
		NewKeyRegistrationTransactionCmdletBase<KeyRegisterOfflineTransaction> {

		protected override void ProcessRecord() {

			var result = CreateTransaction();

			WriteObject(result);
		}

	}

}
