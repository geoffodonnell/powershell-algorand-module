using Algorand.PowerShell.Model;
using System;
using System.Linq;
using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.LogicSignature {

	[Cmdlet(VerbsCommon.New, "AlgorandLogicSignature")]
	public class New_AlgorandLogicSignature : CmdletBase {

		[Parameter(Mandatory = true, ValueFromPipeline = true)]
		public BytesModel Program { get; set; }

		[Parameter(Mandatory = false, ValueFromPipeline = false)]
		public BytesModel[] Arguments { get; set; }

		protected override void ProcessRecord() {

			var program = Program.Bytes;
			var args = Arguments?.Select(s => s.Bytes).ToList();
			var result = new LogicsigSignature { logic = program, args = args };

			WriteObject(result);			
		}

	}

}
