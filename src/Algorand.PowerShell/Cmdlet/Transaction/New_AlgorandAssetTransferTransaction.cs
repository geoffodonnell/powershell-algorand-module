using Algorand.PowerShell.Models;
using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.Transaction {

	[Cmdlet(VerbsCommon.New, "AlgorandAssetTransferTransaction")]
	public class New_AlgorandAssetTransferTransaction : NewTransactionBase {

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
			result.assetCloseTo = AssetCloseTo;

			WriteObject(result);
		}

	}

}
