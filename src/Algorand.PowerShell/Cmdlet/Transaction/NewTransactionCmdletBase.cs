using Algorand.Algod.Model;
using Algorand.PowerShell.Model;
using Org.BouncyCastle.Utilities;
using System;
using System.Management.Automation;
using SdkTransaction = Algorand.Algod.Model.Transactions.Transaction;

namespace Algorand.PowerShell.Cmdlet.Transaction {

	public abstract class NewTransactionCmdletBase<T>
		: CmdletBase where T : SdkTransaction, new() {

		[Parameter(Mandatory = false)]
		public virtual ulong? Fee { get; set; }

		[Parameter(Mandatory = false)]
		public virtual ulong? FirstValid { get; set; }

		[Parameter(Mandatory = false)]
		public virtual BytesModel GenesisHash { get; set; }

		[Parameter(Mandatory = false)]
		public virtual ulong? LastValid { get; set; }

		[Parameter(Mandatory = true)]
		public virtual Address Sender { get; set; }

		[Parameter(Mandatory = false)]
		public virtual string GenesisId { get; set; }

		[Parameter(Mandatory = false)]
		public virtual BytesModel Group { get; set; }

		[Parameter(Mandatory = false)]
		public virtual BytesModel Note { get; set; }

		[Parameter(Mandatory = false)]
		public virtual string NoteAsString { get; set; }

		[Parameter(Mandatory = false)]
		public virtual Address RekeyTo { get; set; }

		protected virtual T CreateTransaction() {

			var result = new T();

			var txParams = GetNetworkParameters();
			var fee = Fee.GetValueOrDefault(txParams.Fee);

			result.Fee = Math.Max(1000, fee);
			result.FirstValid = FirstValid.GetValueOrDefault(txParams.LastRound);
			result.GenesisHash = GetGenesisHash(txParams);
			result.LastValid = LastValid.GetValueOrDefault(txParams.LastRound + 1000);
			result.Sender = Sender;
			result.GenesisID = GetGenesisId(txParams);
			result.Note = GetNote();

			if (Group != null) {
				result.Group = new Digest(Group.Bytes);
			}

			if (RekeyTo != null) {
				result.RekeyTo = RekeyTo;
			}

			return result;
		}

		protected virtual Digest GetGenesisHash(TransactionParametersResponse txParams) {

			if (GenesisHash != null) {
				return new Digest(GenesisHash.Bytes);
			}

			return new Digest(txParams.GenesisHash);
		}

		protected virtual string GetGenesisId(TransactionParametersResponse txParams) {

			if (!String.IsNullOrWhiteSpace(GenesisId)) {
				return GenesisId;
			}

			return txParams.GenesisId;
		}

		protected virtual TransactionParametersResponse GetNetworkParameters() {

			if (Fee.HasValue &&
				FirstValid.HasValue &&
				LastValid.HasValue &&
				GenesisHash != null) {

				return new TransactionParametersResponse {
					Fee = Fee.Value,
					GenesisHash = GenesisHash.Bytes,
					GenesisId = GenesisId ?? String.Empty,
					LastRound = FirstValid.Value,
					MinFee = Fee.Value
				};
			}
			
			return PsEnvironment
				.AlgodDefaultApi
				.TransactionParamsAsync()
				.GetAwaiter()
				.GetResult();			
		}

		protected virtual byte[] GetNote() {

			if (Note != null) {
				return Note.Bytes;
			}

			if (!String.IsNullOrWhiteSpace(NoteAsString)) {
				return Strings.ToUtf8ByteArray(NoteAsString);
			}

			return null;
		}

	}

}
