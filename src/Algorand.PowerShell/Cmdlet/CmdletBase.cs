using Algorand.V2.Algod;
using Algorand.V2.Indexer;

namespace Algorand.PowerShell.Cmdlet {

	public abstract class CmdletBase : System.Management.Automation.Cmdlet {

		protected static Algorand.V2.Algod.ICommonApi AlgodCommonApi => Environment.AlgodCommonApi;

		protected static IDefaultApi AlgodDefaultApi => Environment.AlgodDefaultApi;

		protected static IPrivateApi AlgodPrivateApi => Environment.AlgodPrivateApi;

		protected static Algorand.V2.Indexer.ICommonApi IndexerCommonApi => Environment.IndexerCommonApi;

		protected static ILookupApi IndexerLookupApi => Environment.IndexerLookupApi;

		protected static ISearchApi IndexerSearchApi => Environment.IndexerSearchApi;

		protected CancellationTokenSource CancellationTokenSource { get; private set; }

		protected CancellationToken CancellationToken => CancellationTokenSource.Token;

		protected CmdletBase() { 

			Environment.SafeInitialize();

			CancellationTokenSource = new CancellationTokenSource();
		}

		protected override void StopProcessing() {

			CancellationTokenSource.Cancel();

			base.StopProcessing();
		}

	}

}
