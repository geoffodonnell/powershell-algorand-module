using Algorand.Algod.Model.Transactions;
using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.Transaction {

	[Cmdlet(VerbsCommon.New, "AlgorandAssetFreezeTransaction")]
	public class New_AlgorandAssetFreezeTransaction 
		: NewTransactionCmdletBase<AssetFreezeTransaction> {

		[Parameter(Mandatory = true)]
		public Address FreezeAccount { get; set; }

		[Parameter(Mandatory = true)]
		public ulong? FreezeAsset { get; set; }

		[Parameter(Mandatory = true)]
		public bool? AssetFrozen { get; set; }

		protected override void ProcessRecord() {

			var result = CreateTransaction();

			result.FreezeTarget = FreezeAccount;
			result.AssetFreezeID = FreezeAsset.GetValueOrDefault(0);
			result.FreezeState = AssetFrozen.GetValueOrDefault(false);

			WriteObject(result);
		}

	}

}
