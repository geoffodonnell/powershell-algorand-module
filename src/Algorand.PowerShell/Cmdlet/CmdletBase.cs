using Algorand.V2.Algod;
using Algorand.V2.Indexer;
using System.Threading;

namespace Algorand.PowerShell.Cmdlet {

	public abstract class CmdletBase : System.Management.Automation.Cmdlet {

		protected static Algorand.V2.Algod.ICommonApi AlgodCommonApi => PsEnvironment.AlgodCommonApi;

		protected static IDefaultApi AlgodDefaultApi => PsEnvironment.AlgodDefaultApi;

		protected static IPrivateApi AlgodPrivateApi => PsEnvironment.AlgodPrivateApi;

		protected static Algorand.V2.Indexer.ICommonApi IndexerCommonApi => PsEnvironment.IndexerCommonApi;

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
