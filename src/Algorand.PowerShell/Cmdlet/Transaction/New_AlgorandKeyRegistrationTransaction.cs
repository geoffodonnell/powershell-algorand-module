using Algorand.PowerShell.Model;
using System;
using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.Transaction {

	[Cmdlet(VerbsCommon.New, "AlgorandKeyRegistrationTransaction")]
	public class New_AlgorandKeyRegistrationTransaction : NewTransactionCmdletBase {

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

		[Parameter(Mandatory = false)]
		public bool? Nonparticipation { get; set; }

		protected override void ProcessRecord() {

			if (Nonparticipation != null) {
				WriteError(new ErrorRecord(
					new NotSupportedException($"{nameof(Nonparticipation)} is not supported in this version."),
					String.Empty,
					ErrorCategory.NotSpecified,
					this));
			}

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
