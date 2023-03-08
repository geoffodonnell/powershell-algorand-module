using Algorand.Algod.Model.Transactions;
using Algorand.PowerShell.Model;
using System;
using System.Management.Automation;
using SdkLogicSignature = Algorand.LogicsigSignature;
using SdkTransaction = Algorand.Algod.Model.Transactions.Transaction;

namespace Algorand.PowerShell.Cmdlet.Transaction {

	[Cmdlet(VerbsAlgorand.Sign, "AlgorandTransaction")]
	public class Sign_AlgorandTransaction : CmdletBase {

		[Parameter(
			Mandatory = true,
			ValueFromPipeline = true)]
		public SdkTransaction Transaction { get; set; }

		[Parameter(
			ParameterSetName = "SignWithAccount",
			Mandatory = true,
			ValueFromPipeline = false)]
		public AccountModel Account { get; set; }

		[Parameter(
			ParameterSetName = "SignWithLogicSignature",
			Mandatory = true,
			ValueFromPipeline = false)]
		public SdkLogicSignature LogicSignature { get; set; }

		protected override void ProcessRecord() {

			SignedTransaction result = null;

			if (Account != null) {
				if (!String.Equals(Convert.ToBase64String(Transaction.GenesisHash.Bytes), Account.NetworkGenesisHash)) {

					WriteError(new ErrorRecord(
						new Exception($"Account '{Account.Name}' is not configured for the network this transaction group is targeting."),
						String.Empty,
						ErrorCategory.NotSpecified,
						this));
					return;
				}

				result = SignWithAccount(Account, Transaction);				
			} else if (LogicSignature != null) {
				result = SignLogicsigTransaction(LogicSignature, Transaction);
			}

			WriteObject(result);
		}

		protected static SignedTransaction SignWithAccount(
			AccountModel account, SdkTransaction tx) {

			return account.SignTransaction(tx);
		}

		protected static SignedTransaction SignLogicsigTransaction(
			SdkLogicSignature logicsig, SdkTransaction tx) {

			try {
				return tx.Sign(logicsig);
			} catch (Exception) {
				if (tx.Sender.Equals(logicsig.Address)) {
					return new SignedTransaction() {
						Tx = tx,
						LSig = logicsig
					};
				}

				throw;
			}
		}

	}

}
