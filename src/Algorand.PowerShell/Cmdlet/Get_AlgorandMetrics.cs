using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace Algorand.PowerShell.Cmdlet.Algod {

	[Cmdlet(VerbsCommon.Get, "AlgorandMetrics")]
	public class Get_AlgorandMetrics : CmdletBase {

		protected override void ProcessRecord() {

			WriteError(
				new ErrorRecord(new NotImplementedException(), String.Empty, ErrorCategory.NotSpecified, this));
		}

	}
}
