using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace Algorand.PowerShell.Cmdlet.Transaction {

	[Cmdlet(VerbsAlgorand.Sign, "AlgorandTransaction")]
	public class Sign_AlgorandAccount : CmdletBase {

		protected override void ProcessRecord() {

			WriteError(
				new ErrorRecord(new NotImplementedException(), String.Empty, ErrorCategory.NotSpecified, this));
		}

	}
}
