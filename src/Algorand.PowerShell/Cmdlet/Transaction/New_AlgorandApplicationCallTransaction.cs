using Algorand.PowerShell.Models;
using Algorand.V2.Indexer.Model;
using System;
using System.Linq;
using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.Transaction {

	[Cmdlet(VerbsCommon.New, "AlgorandApplicationCallTransaction")]
	public class New_AlgorandApplicationCallTransaction : NewTransactionBase {

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

			var result = CreateTransaction(Models.TxType.ApplicationCall);

			result.applicationId = ApplicationId.GetValueOrDefault();
			result.onCompletion = OnComplete.ToSdkType();
			result.accounts = Accounts?.ToList();
			result.approvalProgram = ApprovalProgram.Bytes != null ? new TEALProgram(ApprovalProgram.Bytes): null;
			result.applicationArgs = AppArguments?.Select(s => s.Bytes).ToList();
			result.clearStateProgram = ClearStateProgram.Bytes != null ? new TEALProgram(ClearStateProgram.Bytes) : null;
			result.foreignApps = ForeignApps?.ToList();
			result.foreignAssets = ForeignAssets?.ToList();
			result.globalStateSchema = GlobalStateSchema;
			result.localStateSchema = LocalStateSchema;

			WriteObject(result);
		}

	}

}
