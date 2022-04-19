using Algorand.PowerShell.Model;
using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.Transaction {

	[Cmdlet(VerbsCommon.New, "AlgorandAssetFreezeTransaction")]
	public class New_AlgorandAssetFreezeTransaction : NewTransactionCmdletBase {

		[Parameter(Mandatory = true)]
		public Address FreezeAccount { get; set; }

		[Parameter(Mandatory = true)]
		public ulong? FreezeAsset { get; set; }

		[Parameter(Mandatory = true)]
		public bool? AssetFrozen { get; set; }

		protected override void ProcessRecord() {

			var result = CreateTransaction(TxType.AssetFreeze);

			result.freezeTarget = FreezeAccount;
			result.assetFreezeID = FreezeAsset;
			result.freezeState = AssetFrozen.GetValueOrDefault(false);

			WriteObject(result);
		}

	}

}
