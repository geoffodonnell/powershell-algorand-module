using Algorand.V2.Algod.Model;
using System;
using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.Algod {

	[Cmdlet(VerbsCommon.Get, "AlgorandAccountInfo")]
	public class Get_AlgorandAccountInfo : CmdletBase {

		[Parameter(Position = 0, ValueFromPipeline = true)]
		public string Address { get; set; }

		protected override void ProcessRecord() {

			try {
				var result = AlgodDefaultApi
					.AccountsAsync(Address, Format.Json, CancellationToken)
					.GetAwaiter()
					.GetResult();

				WriteObject(result);
			} catch (Exception ex) {
				WriteError(new ErrorRecord(ex, String.Empty, ErrorCategory.NotSpecified, this));
			}
		}

	}

}
