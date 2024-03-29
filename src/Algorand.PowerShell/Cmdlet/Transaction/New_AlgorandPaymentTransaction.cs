﻿using Algorand.Algod.Model.Transactions;
using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.Transaction {

	[Cmdlet(VerbsCommon.New, "AlgorandPaymentTransaction")]
	public class New_AlgorandPaymentTransaction : NewTransactionCmdletBase<PaymentTransaction> {

		[Parameter(Mandatory = true)]
		public Address Receiver { get; set; }

		[Parameter(Mandatory = true)]
		public ulong? Amount { get; set; }

		[Parameter(Mandatory = false)]
		public Address CloseRemainderTo { get; set; }		

		protected override void ProcessRecord() {

			var result = CreateTransaction();

			result.Receiver = Receiver;
			result.Amount = Amount;

			if (CloseRemainderTo != null) {
				result.CloseRemainderTo = CloseRemainderTo;
			}

			WriteObject(result);
		}

	}

}
