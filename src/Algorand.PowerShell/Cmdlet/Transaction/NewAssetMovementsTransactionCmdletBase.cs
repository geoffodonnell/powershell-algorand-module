using Algorand.Algod.Model.Transactions;
using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.Transaction {

	public abstract class NewAssetMovementsTransactionCmdletBase<T>
		: NewTransactionCmdletBase<T> where T : AssetMovementsTransaction, new() {

		[Parameter(Mandatory = true)]
		public virtual ulong? XferAsset { get; set; }

		protected override T CreateTransaction() {

			var result = base.CreateTransaction();

			result.XferAsset = XferAsset.GetValueOrDefault(0);

			return result;
		}

	}

}
