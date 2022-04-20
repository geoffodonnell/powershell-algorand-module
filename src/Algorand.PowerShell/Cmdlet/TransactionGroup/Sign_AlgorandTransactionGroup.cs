using Algorand.PowerShell.Model;
using System;
using System.Linq;
using System.Management.Automation;
using SdkLogicSignature = Algorand.LogicsigSignature;
using SdkTransactionGroup = Algorand.Common.TransactionGroup;

namespace Algorand.PowerShell.Cmdlet.TransactionGroup {

	[Cmdlet(VerbsAlgorand.Sign, "AlgorandTransactionGroup")]
	public class Sign_AlgorandTransactionGroup : CmdletBase {

		[Parameter(
			Mandatory = true,
			ValueFromPipeline = true)]
		public SdkTransactionGroup Group { get; set; }

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

		[Parameter(
			Mandatory = false,
			ValueFromPipeline = false)]
		public SwitchParameter PassThru { get; set; }

		protected override void ProcessRecord() {

			if (Account != null) {

				if (Group.Transactions.Any(
					s => !String.Equals(Convert.ToBase64String(s.genesisHash.Bytes), Account.NetworkGenesisHash))) {

					WriteError(new ErrorRecord(
						new Exception($"Account '{Account.Name}' is not configured for the network this transaction group is targeting."),
						String.Empty,
						ErrorCategory.NotSpecified,
						this));
					return;
				}

				Group.Sign(Account.NetworkAccount);
			}

			if (LogicSignature != null) {
				Group.SignWithLogicSig(LogicSignature);
			}

			if (PassThru.IsPresent && (bool)PassThru) {
				WriteObject(Group);
			}
		}

	}

}
