using Algorand.PowerShell.Model;
using Algorand.V2.Algod.Model;
using Org.BouncyCastle.Utilities;
using System;
using System.Management.Automation;
using SdkAccount = Algorand.Account;
using SdkTransaction = Algorand.Transaction;

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

		protected virtual SdkTransaction CreateTransaction(TxType type) {

			var txParams = GetNetworkParameters();

			var note = GetNote();
			var genesisHash = GetGenesisHash(txParams);
			var genesisId = GetGenesisId(txParams);

			var result = new SdkTransaction() {
				type = type.ToSdkType(),
				fee = Fee.GetValueOrDefault(txParams.Fee),
				firstValid = FirstValid.GetValueOrDefault(txParams.LastRound),
				genesisHash = genesisHash,
				lastValid = LastValid.GetValueOrDefault(txParams.LastRound + 1000),
				sender = Sender,
				genesisID = genesisId,
				note = note
			};

			if (Group != null) {
				result.group = new Digest(Group.Bytes);
			}

			if (RekeyTo != null) {
				result.RekeyTo = RekeyTo;
			}

			SdkAccount.SetFeeByFeePerByte(result, result.fee);

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
