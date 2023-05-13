using Algorand.Algod.Model.Transactions;
using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.Transaction {

	[Cmdlet(VerbsCommon.New, "AlgorandAssetClawbackTransaction")]
	public class New_AlgorandAssetClawbackTransaction 
		: NewAssetMovementsTransactionCmdletBase<AssetClawbackTransaction> {

		[Parameter(Mandatory = true)]
		public ulong? AssetAmount { get; set; }

		[Parameter(Mandatory = true)]
		public Address AssetSender { get; set; }

		[Parameter(Mandatory = true)]
		public Address AssetReceiver { get; set; }

		protected override void ProcessRecord() {

			var result = CreateTransaction();

			result.XferAsset = XferAsset.GetValueOrDefault(0);
			result.AssetAmount = AssetAmount.GetValueOrDefault(0);

			if (AssetSender != null) {
				result.AssetSender = AssetSender;
			}

			if (AssetReceiver != null) {
				result.AssetReceiver = AssetReceiver;
			}

			WriteObject(result);
		}

	}

}
