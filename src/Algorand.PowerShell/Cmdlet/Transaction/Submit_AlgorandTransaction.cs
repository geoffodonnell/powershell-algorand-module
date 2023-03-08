using Algorand.Common;
using Algorand.Algod;
using Algorand.Algod.Model;
using System;
using System.Management.Automation;
using System.Threading.Tasks;
using Algorand.Algod.Model.Transactions;
using System.Collections.Generic;

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

			// Wait for confirmation by default
			var wait = NoWait.IsPresent ? !(bool)NoWait : true;

			PostTransactionsResponse result = null;

			try {				
				result = SubmitTransaction(AlgodDefaultApi, Transaction)
					.GetAwaiter()
					.GetResult();

				if (wait) {
					AlgodDefaultApi
						.WaitForTransactionToComplete(result.Txid)
						.GetAwaiter()
						.GetResult();
				}

				WriteObject(result);
			} catch (ApiException ex) {
				WriteError(new ErrorRecord(
					ex.GetExceptionWithBetterMessage(), String.Empty, ErrorCategory.NotSpecified, this));

			//Handling for: https://github.com/FrankSzendzielarz/dotnet-algorand-sdk/issues/11
			} catch (FormatException ex) {
				if (result != null) {
					WriteObject(result);
				} else {
					WriteError(new ErrorRecord(ex, String.Empty, ErrorCategory.NotSpecified, this));
				}
			} catch (Exception ex) {
				WriteError(new ErrorRecord(ex, String.Empty, ErrorCategory.NotSpecified, this));
			}
		}

		protected static async Task<PostTransactionsResponse> SubmitTransaction(
			IDefaultApi instance,
			SignedTransaction signedTx) {

			return await instance.TransactionsAsync(
				new List<SignedTransaction>() {
					signedTx
				});			
		}

	}

}
