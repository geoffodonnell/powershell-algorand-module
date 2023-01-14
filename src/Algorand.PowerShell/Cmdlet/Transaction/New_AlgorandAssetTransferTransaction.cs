using Algorand.Algod.Model.Transactions;
using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.Transaction {

	[Cmdlet(VerbsCommon.New, "AlgorandAssetTransferTransaction")]
	public class New_AlgorandAssetTransferTransaction
		: NewAssetMovementsTransactionCmdletBase<AssetTransferTransaction> {

		[Parameter(Mandatory = true)]
		public ulong? AssetAmount { get; set; }

		[Parameter(Mandatory = true)]
		public Address AssetReceiver { get; set; }

		[Parameter(Mandatory = false)]
		public Address AssetCloseTo { get; set; }

		protected override void ProcessRecord() {

			var result = CreateTransaction();

			result.XferAsset = XferAsset;
			result.AssetAmount = AssetAmount.GetValueOrDefault(0);
			result.AssetReceiver = AssetReceiver;

			if (AssetCloseTo != null) {
				result.AssetCloseTo = AssetCloseTo;
			}

			WriteObject(result);
		}

	}

}
