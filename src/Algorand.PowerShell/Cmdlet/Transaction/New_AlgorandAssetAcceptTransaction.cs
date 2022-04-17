using Algorand.PowerShell.Models;
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
			result.assetSender = AssetSender;
			result.assetReceiver = AssetReceiver;

			WriteObject(result);
		}

	}

}
