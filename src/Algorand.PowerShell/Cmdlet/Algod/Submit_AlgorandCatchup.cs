﻿using System;
using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.Algod {

	[Cmdlet(VerbsLifecycle.Submit, "AlgorandCatchup")]
	public class Submit_AlgorandCatchup : CmdletBase {

		[Parameter(
			Position = 0,
			Mandatory = true, 
			ValueFromPipeline = true)]
		public string Catchpoint { get; set; }

		protected override void ProcessRecord() {

			try {
				throw new NotSupportedException(
					"This version of the PowerShell module does not support private API endpoints.");
			} catch (ApiException ex) {
				WriteError(new ErrorRecord(
					ex.GetExceptionWithBetterMessage(), String.Empty, ErrorCategory.NotSpecified, this));
			} catch (Exception ex) {
				WriteError(new ErrorRecord(ex, String.Empty, ErrorCategory.NotSpecified, this));
			}
		}

	}

}
