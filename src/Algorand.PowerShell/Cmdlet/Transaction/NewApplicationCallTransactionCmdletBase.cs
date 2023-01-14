using Algorand.Algod.Model.Transactions;
using Algorand.PowerShell.Model;
using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.Transaction {

	public abstract class NewApplicationCallTransactionCmdletBase<T>
		: NewTransactionCmdletBase<T> where T : ApplicationCallTransaction, new() {

		[Parameter(Mandatory = false)]
		public virtual Address[] Accounts { get; set; }

		[Parameter(Mandatory = false)]
		public virtual BytesModel[] AppArguments { get; set; }

		[Parameter(Mandatory = false)]
		public virtual ulong[] ForeignApps { get; set; }

		[Parameter(Mandatory = false)]
		public virtual ulong[] ForeignAssets { get; set; }

		protected override T CreateTransaction() {

			return base.CreateTransaction();
		}

	}

}
