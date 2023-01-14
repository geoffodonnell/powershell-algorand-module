using Algorand.Algod.Model.Transactions;

namespace Algorand.PowerShell.Cmdlet.Transaction {
	public abstract class NewAssetConfigurationTransactionCmdletBase<T>
		: NewTransactionCmdletBase<T> where T : AssetConfigurationTransaction, new() {
	}

}
