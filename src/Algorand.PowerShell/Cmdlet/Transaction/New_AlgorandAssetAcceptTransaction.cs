using Algorand.PowerShell.Model;
using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.Transaction {

	[Cmdlet(VerbsCommon.New, "AlgorandAssetAcceptTransaction")]
	public class New_AlgorandAssetAcceptTransaction : NewTransactionCmdletBase {

		[Parameter(Mandatory = false)]
		public ulong? XferAsset { get; set; }

		[Parameter(Mandatory = false)]
		public Address AssetSender { get; set; }

		[Parameter(Mandatory = false)]
		public Address AssetReceiver { get; set; }

		protected override void ProcessRecord() {

			var result = CreateTransaction(TxType.AssetTransfer);

			result.xferAsset = XferAsset;

			if (AssetSender != null) {
				result.assetSender = AssetSender;
			}

			if (AssetReceiver != null) {
				result.assetReceiver = AssetReceiver;
			}

			WriteObject(result);
		}

	}

}
