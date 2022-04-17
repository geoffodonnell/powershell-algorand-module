using System;
using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.Transaction {

	[Cmdlet(VerbsAlgorand.Sign, "AlgorandTransaction")]
	public class Sign_AlgorandTransaction : CmdletBase {

		[Parameter(
			Mandatory = true,
			ValueFromPipeline = true)]
		public Algorand.Transaction Transaction { get; set; }

		[Parameter(
			ParameterSetName = "SignWithAccount",
			Mandatory = true,
			ValueFromPipeline = false)]
		public Algorand.Account Account { get; set; }

		[Parameter(
			ParameterSetName = "SignWithLogicSignature",
			Mandatory = true,
			ValueFromPipeline = false)]
		public Algorand.LogicsigSignature LogicSignature { get; set; }

		protected override void ProcessRecord() {

			SignedTransaction result = null;

			if (Account != null) {
				result = Account.SignTransaction(Transaction);
			} else if (LogicSignature != null) {
				result = SignLogicsigTransaction(LogicSignature, Transaction);
			}

			WriteObject(result);
		}

		protected static SignedTransaction SignLogicsigTransaction(
			LogicsigSignature logicsig, Algorand.Transaction tx) {

			try {
				return Algorand.Account.SignLogicsigTransaction(logicsig, tx);
			} catch (Exception) {
				if (tx.sender.Equals(logicsig.Address)) {
					return new SignedTransaction(tx, logicsig, tx.TxID());
				}

				throw;
			}
		}

	}

}
