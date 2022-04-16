using Algorand.PowerShell.Models;
using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.Transaction {

	[Cmdlet(VerbsCommon.New, "AlgorandAssetTransferTransaction")]
	public class New_AlgorandAssetTransferTransaction : NewTransactionBase {

		[Parameter(Mandatory = false)]
		public ulong? XferAsset { get; set; }

		[Parameter(Mandatory = false)]
		public ulong? AssetAmount { get; set; }

		[Parameter(Mandatory = false)]
		public Address AssetSender { get; set; }

		[Parameter(Mandatory = false)]
		public Address AssetReceiver { get; set; }

		[Parameter(Mandatory = false)]
		public Address AssetCloseTo { get; set; }

		protected override void ProcessRecord() {

			var result = CreateTransaction(TxType.KeyRegistration);

			result.xferAsset = XferAsset;
			result.assetAmount = AssetAmount;
			result.assetSender = AssetSender;
			result.assetReceiver = AssetReceiver;
			result.assetCloseTo = AssetCloseTo;

			WriteObject(result);
		}

	}

}
