using Algorand.Algod.Model.Transactions;
using Algorand.PowerShell.Model;
using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.Transaction {

	[Cmdlet(VerbsCommon.New, "AlgorandKeyRegisterOnlineTransaction")]
	public class New_AlgorandKeyRegisterOnlineTransaction 
		: NewKeyRegistrationTransactionCmdletBase<KeyRegisterOnlineTransaction> {

		[Parameter(Mandatory = true)]
		public BytesModel VotePk { get; set; }

		[Parameter(Mandatory = true)]
		public BytesModel SelectionPk { get; set; }

		[Parameter(Mandatory = true)]
		public ulong VoteFirst { get; set; }

		[Parameter(Mandatory = true)]
		public ulong VoteLast { get; set; }

		[Parameter(Mandatory = true)]
		public ulong VoteKeyDilution { get; set; }

		protected override void ProcessRecord() {

			var result = CreateTransaction();

			if (VotePk != null) {
				result.Votepk = new ParticipationPublicKey(VotePk.Bytes);
			}

			if (SelectionPk != null) {
				result.SelectionPk = new VRFPublicKey(SelectionPk.Bytes);
			}

			result.VoteFirst = VoteFirst;
			result.VoteLast = VoteLast;
			result.VoteKeyDilution = VoteKeyDilution;

			WriteObject(result);
		}

	}

}
