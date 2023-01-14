using Algorand.Algod.Model.Transactions;
using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.Transaction {
	public abstract class NewAssetChangeTransactionCmdletBase<T>
		: NewAssetConfigurationTransactionCmdletBase<T> where T : AssetChangeTransaction, new() {

		[Parameter(Mandatory = true)]
		public virtual ulong? AssetIndex { get; set; }

		protected override T CreateTransaction() {

			var result = base.CreateTransaction();

			if (AssetIndex.HasValue) {
				result.AssetIndex = AssetIndex.Value;
			}

			return result;
		}
	}

}
