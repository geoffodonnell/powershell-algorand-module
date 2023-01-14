using Algorand.Algod.Model.Transactions;
using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.Transaction {

	[Cmdlet(VerbsCommon.New, "AlgorandAssetDestroyTransaction")]
	public class New_AlgorandAssetDestroyTransaction 
		: NewAssetChangeTransactionCmdletBase<AssetDestroyTransaction> {

		protected override void ProcessRecord() {

			var result = CreateTransaction();

			WriteObject(result);
		}
		
	}

}
