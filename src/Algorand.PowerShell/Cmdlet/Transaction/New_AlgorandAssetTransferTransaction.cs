using Algorand.PowerShell.Model;
using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.Transaction {

	[Cmdlet(VerbsCommon.New, "AlgorandAssetTransferTransaction")]
	public class New_AlgorandAssetTransferTransaction : NewTransactionCmdletBase {

		[Parameter(Mandatory = true)]
		public ulong? XferAsset { get; set; }

		[Parameter(Mandatory = true)]
		public ulong? AssetAmount { get; set; }

		[Parameter(Mandatory = true)]
		public Address AssetReceiver { get; set; }

		[Parameter(Mandatory = false)]
		public Address AssetCloseTo { get; set; }

		protected override void ProcessRecord() {

			var result = CreateTransaction(TxType.AssetTransfer);

			result.xferAsset = XferAsset;
			result.assetAmount = AssetAmount;
			result.assetReceiver = AssetReceiver;

			if (AssetCloseTo != null) {
				result.assetCloseTo = AssetCloseTo;
			}

			WriteObject(result);
		}

	}

}
