using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.TransactionGroup {

	[Cmdlet(VerbsAlgorand.Sign, "AlgorandTransactionGroup")]
	public class Sign_AlgorandTransactionGroup : CmdletBase {

		[Parameter(
			Mandatory = true,
			ValueFromPipeline = true)]
		public Algorand.Common.TransactionGroup Group { get; set; }

		[Parameter(
			ParameterSetName = "SignWithAccount",
			Mandatory = true,
			ValueFromPipeline = false)]
		public Algorand.Account Account { get; set; }

		[Parameter(
			ParameterSetName = "SignWithLogicSignature",
			Mandatory = true,
			ValueFromPipeline = false)]
		public Algorand.LogicsigSignature LogicSignature { get; set; }

		[Parameter(
			Mandatory = false,
			ValueFromPipeline = false)]
		public SwitchParameter PassThru { get; set; }

		protected override void ProcessRecord() {

			if (Account != null) {
				Group.Sign(Account);
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
