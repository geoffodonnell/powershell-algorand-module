using Algorand.PowerShell.Model;
using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.Transaction {

	[Cmdlet(VerbsCommon.New, "AlgorandAssetClawbackTransaction")]
	public class New_AlgorandAssetClawbackTransaction : NewTransactionCmdletBase {

		[Parameter(Mandatory = true)]
		public ulong? XferAsset { get; set; }

		[Parameter(Mandatory = true)]
		public ulong? AssetAmount { get; set; }

		[Parameter(Mandatory = true)]
		public Address AssetSender { get; set; }

		[Parameter(Mandatory = true)]
		public Address AssetReceiver { get; set; }

		[Parameter(Mandatory = false)]
		public Address AssetCloseTo { get; set; }

		protected override void ProcessRecord() {

			var result = CreateTransaction(TxType.AssetTransfer);

			result.xferAsset = XferAsset;
			result.assetAmount = AssetAmount;

			if (AssetSender != null) {
				result.assetSender = AssetSender;
			}

			if (AssetReceiver != null) {
				result.assetReceiver = AssetReceiver;
			}

			if (AssetCloseTo != null) {
				result.assetCloseTo = AssetCloseTo;
			}

			WriteObject(result);
		}

	}

}
