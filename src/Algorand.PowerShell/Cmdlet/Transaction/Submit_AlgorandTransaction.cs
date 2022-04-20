using Algorand.Common;
using Algorand.V2.Algod;
using Algorand.V2.Algod.Model;
using System;
using System.IO;
using System.Management.Automation;
using System.Threading.Tasks;

namespace Algorand.PowerShell.Cmdlet.Transaction {

	[Cmdlet(VerbsLifecycle.Submit, "AlgorandTransaction")]
	public class Submit_AlgorandTransaction : CmdletBase {

		[Parameter(
			Mandatory = true,
			ValueFromPipeline = true)]
		public SignedTransaction Transaction { get; set; }

		[Parameter(
			Mandatory = false,
			ValueFromPipeline = false)]
		public SwitchParameter NoWait { get; set; }

		protected override void ProcessRecord() {

			try {

				// Wait for confirmation by default
				var wait = NoWait.IsPresent ? !(bool)NoWait : true;
				var result = SubmitTransaction(AlgodDefaultApi, Transaction)
					.GetAwaiter()
					.GetResult();

				if (wait) {
					AlgodDefaultApi
						.WaitForTransactionToComplete(result.TxId)
						.GetAwaiter()
						.GetResult();	
				}

				WriteObject(result);
			} catch (Exception ex) {
				WriteError(new ErrorRecord(ex, String.Empty, ErrorCategory.NotSpecified, this));
			}
		}

		// Copied from SDK to support interface over impl type
		protected static async Task<PostTransactionsResponse> SubmitTransaction(
			IDefaultApi instance,
			SignedTransaction signedTx) {

			byte[] encodedTxBytes = Encoder.EncodeToMsgPack(signedTx);
			using (MemoryStream ms = new MemoryStream(encodedTxBytes)) {
				return await instance.TransactionsAsync(ms);
			}
		}

	}

}
