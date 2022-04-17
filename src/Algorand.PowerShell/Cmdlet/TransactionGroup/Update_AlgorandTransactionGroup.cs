using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.TransactionGroup {

	[Cmdlet(VerbsData.Update, "AlgorandTransactionGroup")]
	public class Update_AlgorandTransactionGroup : CmdletBase {

		// TODO: Update TransactionGroup to support remove?

		[Parameter(
			Mandatory = true)]
		public SwitchParameter Add { get; set; }

		[Parameter(
			Mandatory = true,
			ValueFromPipeline = true)]
		public Algorand.Common.TransactionGroup Group { get; set; }

		[Parameter (
			Mandatory = true,
			ValueFromPipeline = false)]
		public Algorand.Transaction Transaction { get; set; }

		[Parameter(
			Mandatory = false,
			ValueFromPipeline = false)]
		public SwitchParameter PassThru { get; set; }

		protected override void ProcessRecord() {

			if (Add.IsPresent && (bool)Add) {
				Group.Add(Transaction);
			}

			if (PassThru.IsPresent && (bool)PassThru) {
				WriteObject(Group);
			}
		}

	}

}
