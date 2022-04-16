using Algorand.PowerShell.Models;
using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.Transaction {

	[Cmdlet(VerbsCommon.New, "AlgorandPaymentTransaction")]
	public class New_AlgorandPaymentTransaction : NewTransactionBase {

		[Parameter(Mandatory = true)]
		public Address Receiver { get; set; }

		[Parameter(Mandatory = true)]
		public ulong? Amount { get; set; }

		[Parameter(Mandatory = false)]
		public Address CloseRemainderTo { get; set; }		

		protected override void ProcessRecord() {

			var result = CreateTransaction(TxType.Payment);

			result.receiver = Receiver;
			result.amount = Amount;
			result.closeRemainderTo = CloseRemainderTo;

			WriteObject(result);
		}

	}

}
