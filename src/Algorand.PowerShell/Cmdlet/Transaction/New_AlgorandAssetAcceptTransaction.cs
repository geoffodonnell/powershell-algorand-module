using Algorand.Algod.Model.Transactions;
using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.Transaction {

	[Cmdlet(VerbsCommon.New, "AlgorandAssetAcceptTransaction")]
	public class New_AlgorandAssetAcceptTransaction : 
		NewAssetMovementsTransactionCmdletBase<AssetAcceptTransaction>  {

		[Parameter(Mandatory = true, ParameterSetName = "Default")]
		public override Address Sender { get; set; }

		[Parameter(Mandatory = true, ParameterSetName = "Default")]
		public Address AssetReceiver { get; set; }

		[Parameter(Mandatory = true, ParameterSetName = "Address")]
		public Address Address { get; set; }

		protected override void ProcessRecord() {

			var result = CreateTransaction();

			result.XferAsset = XferAsset;
			result.Sender = Sender ?? Address;
			result.AssetReceiver = AssetReceiver ?? Address;			

			WriteObject(result);
		}

	}

}
