﻿using System;
using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.Indexer {

	[Cmdlet(VerbsCommon.Find, "AlgorandBlock")]
	public class Find_AlgorandBlock : CmdletBase {

		[Parameter(
			Mandatory = true,
			ValueFromPipeline = false)]
		public ulong? Round { get; set; }

		protected override void ProcessRecord() {

			try {
				var result = IndexerLookupApi
					.lookupBlockAsync(
						CancellationToken,
						Round.GetValueOrDefault())
					.GetAwaiter()
					.GetResult();

				WriteObject(result);
			} catch (ApiException ex) {
				WriteError(new ErrorRecord(
					ex.GetExceptionWithBetterMessage(), String.Empty, ErrorCategory.NotSpecified, this));
			} catch (Exception ex) {
				WriteError(new ErrorRecord(ex, String.Empty, ErrorCategory.NotSpecified, this));
			}
		}

	}

}
