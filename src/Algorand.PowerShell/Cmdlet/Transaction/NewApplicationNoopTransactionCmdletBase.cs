using Algorand.Algod.Model.Transactions;
using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.Transaction {

	public abstract class NewApplicationNoopTransactionCmdletBase<T>
		: NewApplicationCallTransactionCmdletBase<T> where T : ApplicationNoopTransaction, new() {

		[Parameter(Mandatory = true)]
		public virtual  ulong? ApplicationId { get; set; }

		protected override T CreateTransaction() {

			var result =  base.CreateTransaction();

			result.ApplicationId = ApplicationId.GetValueOrDefault(0);

			return result;
		}

	}

}
