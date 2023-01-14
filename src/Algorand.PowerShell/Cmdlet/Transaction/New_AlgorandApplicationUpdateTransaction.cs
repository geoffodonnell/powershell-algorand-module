using Algorand.Algod.Model.Transactions;
using Algorand.PowerShell.Model;
using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.Transaction {

	[Cmdlet(VerbsCommon.New, "AlgorandApplicationUpdateTransaction")]
	public class New_AlgorandApplicationUpdateTransaction
		: NewApplicationCallTransactionCmdletBase<ApplicationUpdateTransaction> {

		[Parameter(Mandatory = true)]
		public ulong? ApplicationId { get; set; }

		[Parameter(Mandatory = false)]
		public BytesModel ApprovalProgram { get; set; }

		[Parameter(Mandatory = false)]
		public BytesModel ClearStateProgram { get; set; }

		[Parameter(Mandatory = false)]
		public StateSchema GlobalStateSchema { get; set; }

		[Parameter(Mandatory = false)]
		public StateSchema LocalStateSchema { get; set; }

		[Parameter(Mandatory = false)]
		public ulong? ExtraProgramPages { get; set; }

		protected override void ProcessRecord() {

			var result = CreateTransaction();

			result.ApplicationId = ApplicationId;
			result.ExtraProgramPages = ExtraProgramPages;

			if (ApprovalProgram != null) {
				result.ApprovalProgram = new TEALProgram(ApprovalProgram.Bytes);
			}

			if (ClearStateProgram != null) {
				result.ClearStateProgram = new TEALProgram(ClearStateProgram.Bytes);
			}

			if (GlobalStateSchema != null) {
				result.GlobalStateSchema = GlobalStateSchema;
			}

			if (LocalStateSchema != null) {
				result.LocalStateSchema = LocalStateSchema;
			}

			WriteObject(result);
		}

	}

}
