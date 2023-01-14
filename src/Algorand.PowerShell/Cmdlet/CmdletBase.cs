using Algorand.Algod;
using Algorand.Indexer;
using System.Threading;

namespace Algorand.PowerShell.Cmdlet {

	public abstract class CmdletBase : System.Management.Automation.Cmdlet {

		protected static IDefaultApi AlgodDefaultApi => PsEnvironment.AlgodDefaultApi;

		protected static ICommonApi IndexerCommonApi => PsEnvironment.IndexerCommonApi;

		protected static ILookupApi IndexerLookupApi => PsEnvironment.IndexerLookupApi;

		protected static ISearchApi IndexerSearchApi => PsEnvironment.IndexerSearchApi;

		protected CancellationTokenSource CancellationTokenSource { get; private set; }

		protected CancellationToken CancellationToken => CancellationTokenSource.Token;

		protected CmdletBase() { 

			PsEnvironment.SafeInitialize();

			CancellationTokenSource = new CancellationTokenSource();
		}

		protected override void StopProcessing() {

			CancellationTokenSource.Cancel();

			base.StopProcessing();
		}

	}

}
