using Algorand.PowerShell.Model;
using Algorand.V2.Indexer.Model;
using System;
using System.Linq;
using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.Transaction {

	[Cmdlet(VerbsCommon.New, "AlgorandApplicationCallTransaction")]
	public class New_AlgorandApplicationCallTransaction : NewTransactionCmdletBase {

		[Parameter(Mandatory = true)]
		public ulong? ApplicationId { get; set; }

		[Parameter(Mandatory = false)]
		public OnComplete? OnComplete { get; set; }

		[Parameter(Mandatory = false)]
		public Address[] Accounts { get; set; }

		[Parameter(Mandatory = false)]
		public BytesModel ApprovalProgram { get; set; }

		[Parameter(Mandatory = false)]
		public BytesModel[] AppArguments { get; set; }

		[Parameter(Mandatory = false)]
		public BytesModel ClearStateProgram { get; set; }

		[Parameter(Mandatory = false)]
		public ulong[] ForeignApps { get; set; }

		[Parameter(Mandatory = false)]
		public ulong[] ForeignAssets { get; set; }

		[Parameter(Mandatory = false)]
		public StateSchema GlobalStateSchema { get; set; }

		[Parameter(Mandatory = false)]
		public StateSchema LocalStateSchema { get; set; }

		[Parameter(Mandatory = false)]
		public BytesModel[] ExtraProgramPages { get; set; }

		protected override void ProcessRecord() {

			if (ExtraProgramPages != null) {
				WriteError(new ErrorRecord(
					new NotSupportedException($"{nameof(ExtraProgramPages)} is not supported in this version."),
					String.Empty, 
					ErrorCategory.NotSpecified, 
					this));
			}

			var result = CreateTransaction(Model.TxType.ApplicationCall);

			result.applicationId = ApplicationId.GetValueOrDefault();
			result.onCompletion = OnComplete.ToSdkType();

			if (Accounts != null) {
				result.accounts = Accounts.ToList();
			}

			if (ApprovalProgram != null) {
				result.approvalProgram = new TEALProgram(ApprovalProgram.Bytes);
			}

			if (AppArguments != null) {
				result.applicationArgs = AppArguments.Select(s => s.Bytes).ToList();
			}

			if (ClearStateProgram != null) {
				result.clearStateProgram = new TEALProgram(ClearStateProgram.Bytes);
			}

			if (ForeignApps != null) {
				result.foreignApps = ForeignApps.ToList();
			}

			if (ForeignAssets != null) {
				result.foreignAssets = ForeignAssets?.ToList();
			}

			if (GlobalStateSchema != null) {
				result.globalStateSchema = GlobalStateSchema;
			}

			if (LocalStateSchema != null) {
				result.localStateSchema = LocalStateSchema;
			}

			WriteObject(result);
		}

	}

}
