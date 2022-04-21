using System;
using System.Management.Automation;
using SdkTransaction = Algorand.Transaction;

namespace Algorand.PowerShell.Cmdlet.Transaction {

	[Cmdlet(VerbsData.ConvertTo, "AlgorandTransaction")]
	public class ConvertTo_AlgorandTransaction : CmdletBase {

		[Parameter(
			Mandatory = true,
			ValueFromPipeline = true)]
		public string Json { get; set; }

		protected override void ProcessRecord() {

			if (String.IsNullOrWhiteSpace(Json)) {
				WriteError(new ErrorRecord(
					new Exception($"{nameof(Json)} must be valid JSON object."),
					String.Empty,
					ErrorCategory.NotSpecified,
					this));
				return;
			}

			var result = Encoder.DecodeFromJson<SdkTransaction>(Json);

			WriteObject(result);
		}

	}

}
