using Algorand.PowerShell.Models;
using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.Transaction {

	[Cmdlet(VerbsCommon.New, "AlgorandKeyRegistrationTransaction")]
	public class New_AlgorandKeyRegistrationTransaction : NewTransactionBase {

		[Parameter(Mandatory = false)]
		public BytesModel VotePk { get; set; }

		[Parameter(Mandatory = false)]
		public BytesModel SelectionPk { get; set; }

		[Parameter(Mandatory = false)]
		public ulong VoteFirst { get; set; }

		[Parameter(Mandatory = false)]
		public ulong VoteLast { get; set; }

		[Parameter(Mandatory = false)]
		public ulong VoteKeyDilution { get; set; }

		protected override void ProcessRecord() {

			var result = CreateTransaction(TxType.KeyRegistration);

			result.votePK = new ParticipationPublicKey(VotePk.Bytes);
			result.selectionPK = new VRFPublicKey(SelectionPk.Bytes);
			result.voteFirst = VoteFirst;
			result.voteLast = VoteLast;
			result.voteKeyDilution = VoteKeyDilution;

			WriteObject(result);
		}

	}

}
