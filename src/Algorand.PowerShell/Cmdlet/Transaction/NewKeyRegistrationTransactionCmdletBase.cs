using Algorand.Algod.Model.Transactions;

namespace Algorand.PowerShell.Cmdlet.Transaction {
	public abstract class NewKeyRegistrationTransactionCmdletBase<T>
		: NewTransactionCmdletBase<T> where T : KeyRegistrationTransaction, new() {
	}

}
