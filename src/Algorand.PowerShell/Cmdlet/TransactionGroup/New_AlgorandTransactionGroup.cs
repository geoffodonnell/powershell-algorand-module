using System;
using System.Management.Automation;
using SdkTransaction = Algorand.Algod.Model.Transactions.Transaction;

namespace Algorand.PowerShell.Cmdlet.TransactionGroup {

	[Cmdlet(VerbsCommon.New, "AlgorandTransactionGroup")]
	public class New_AlgorandTransactionGroup : CmdletBase {

		protected override void ProcessRecord() {

			WriteObject(
				new Algorand.Common.TransactionGroup(Array.Empty<SdkTransaction>()));
		}

	}

}
