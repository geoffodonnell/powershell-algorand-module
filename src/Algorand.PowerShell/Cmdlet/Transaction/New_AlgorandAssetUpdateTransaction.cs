using Algorand.Algod.Model;
using Algorand.Algod.Model.Transactions;
using Algorand.PowerShell.Model;
using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.Transaction {

	[Cmdlet(VerbsCommon.New, "AlgorandAssetUpdateTransaction")]
	public class New_AlgorandAssetUpdateTransaction 
		: NewAssetChangeTransactionCmdletBase<AssetUpdateTransaction> {

		[Parameter(Mandatory = false)]
		public ulong? Total { get; set; }

		[Parameter(Mandatory = false)]
		public ulong? Decimals { get; set; }

		[Parameter(Mandatory = false)]
		public bool? DefaultFrozen { get; set; }

		[Parameter(Mandatory = false)]
		public string UnitName { get; set; }

		[Parameter(Mandatory = false)]
		public string AssetName { get; set; }

		[Parameter(Mandatory = false)]
		public string Url { get; set; }

		[Parameter(Mandatory = false)]
		public BytesModel MetaDataHash { get; set; }

		[Parameter(Mandatory = false)]
		public Address ManagerAddr { get; set; }

		[Parameter(Mandatory = false)]
		public Address ReserveAddr { get; set; }

		[Parameter(Mandatory = false)]
		public Address FreezeAddr { get; set; }

		[Parameter(Mandatory = false)]
		public Address ClawbackAddr { get; set; }

		protected override void ProcessRecord() {

			var result = CreateTransaction();

			result.AssetIndex = AssetIndex.Value;
			result.AssetParams = new AssetParams {
				Total = Total,
				Decimals = Decimals.GetValueOrDefault(),
				DefaultFrozen = DefaultFrozen.GetValueOrDefault()
			};

			if (UnitName != null) {
				result.AssetParams.UnitName = UnitName;
			}

			if (AssetName != null) {
				result.AssetParams.Name = AssetName;
			}

			if (Url != null) {
				result.AssetParams.Url = Url;
			}

			if (MetaDataHash != null) {
				result.AssetParams.MetadataHash = MetaDataHash.Bytes;
			}

			if (ManagerAddr != null) {
				result.AssetParams.Manager = ManagerAddr;
			}

			if (ReserveAddr != null) {
				result.AssetParams.Reserve = ReserveAddr;
			}

			if (FreezeAddr != null) {
				result.AssetParams.Freeze = FreezeAddr;
			}

			if (ClawbackAddr != null) {
				result.AssetParams.Clawback = ClawbackAddr;
			}

			WriteObject(result);
		}
		
	}

}
