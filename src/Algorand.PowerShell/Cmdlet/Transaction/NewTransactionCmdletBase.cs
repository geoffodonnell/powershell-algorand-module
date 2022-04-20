using Algorand.PowerShell.Model;
using Algorand.V2.Algod.Model;
using Org.BouncyCastle.Utilities;
using System;
using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.Transaction {

	public abstract class NewTransactionCmdletBase : CmdletBase {

		[Parameter(Mandatory = false)]
		public ulong? Fee { get; set; }

		[Parameter(Mandatory = false)]
		public ulong? FirstValid { get; set; }

		[Parameter(Mandatory = false)]
		public BytesModel GenesisHash { get; set; }

		[Parameter(Mandatory = false)]
		public ulong? LastValid { get; set; }

		[Parameter(Mandatory = true)]
		public Address Sender { get; set; }

		[Parameter(Mandatory = false)]
		public string GenesisId { get; set; }

		[Parameter(Mandatory = false)]
		public BytesModel Group { get; set; }

		[Parameter(Mandatory = false)]
		public BytesModel Note { get; set; }

		[Parameter(Mandatory = false)]
		public string NoteAsString { get; set; }

		[Parameter(Mandatory = false)]
		public Address RekeyTo { get; set; }

		protected virtual Algorand.Transaction CreateTransaction(TxType type) {

			var txParams = GetNetworkParameters();

			var note = GetNote();
			var genesisHash = GetGenesisHash(txParams);

			var result = new Algorand.Transaction() {
				type = type.ToSdkType(),
				fee = Fee.GetValueOrDefault(txParams.Fee),
				firstValid = FirstValid.GetValueOrDefault(txParams.LastRound),
				genesisHash = genesisHash,
				lastValid = LastValid.GetValueOrDefault(txParams.LastRound + 1000),
				sender = Sender,
				genesisID = GenesisId,
				group = Group != null ? new Digest(Group.Bytes) : null,
				note = note,
				RekeyTo = RekeyTo
			};

			return result;
		}

		protected virtual Digest GetGenesisHash(TransactionParametersResponse txParams) {

			if (GenesisHash != null) {
				return new Digest(GenesisHash.Bytes);
			}

			return new Digest(txParams.GenesisHash);
		}

		protected virtual TransactionParametersResponse GetNetworkParameters() {

			if (Fee.HasValue &&
				FirstValid.HasValue &&
				LastValid.HasValue &&
				GenesisHash != null) {

				return new TransactionParametersResponse {
					Fee = Fee.Value,
					GenesisHash = GenesisHash.Bytes,
					GenesisId = GenesisId,
					LastRound = FirstValid.Value,
					MinFee = Fee.Value
				};
			}
			
			return PsEnvironment
				.AlgodDefaultApi
				.ParamsAsync()
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
