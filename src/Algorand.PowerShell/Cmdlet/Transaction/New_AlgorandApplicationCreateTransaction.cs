using Algorand.Algod.Model.Transactions;
using Algorand.PowerShell.Model;
using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.Transaction {

	[Cmdlet(VerbsCommon.New, "AlgorandApplicationCreateTransaction")]
	public class New_AlgorandApplicationCreateTransaction
		: NewApplicationNoopTransactionCmdletBase<ApplicationCreateTransaction> {

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
