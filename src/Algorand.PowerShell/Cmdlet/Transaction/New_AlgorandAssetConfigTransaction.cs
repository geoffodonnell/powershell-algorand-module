using Algorand.PowerShell.Model;
using System.Management.Automation;

namespace Algorand.PowerShell.Cmdlet.Transaction {

	[Cmdlet(VerbsCommon.New, "AlgorandAssetConfigTransaction")]
	public class New_AlgorandAssetConfigTransaction : NewTransactionCmdletBase {

		[Parameter(Mandatory = false)]
		public ulong? ConfigAsset { get; set; }

		[Parameter(Mandatory = false)]
		public ulong? Total { get; set; }

		[Parameter(Mandatory = false)]
		public int? Decimals { get; set; }

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

			var result = CreateTransaction(TxType.KeyRegistration);

			result.assetIndex = ConfigAsset;
			result.assetParams = new Algorand.Transaction.AssetParams {
				assetTotal = Total,
				assetDecimals = Decimals.GetValueOrDefault(),
				assetDefaultFrozen = DefaultFrozen.GetValueOrDefault()
			};

			if (UnitName != null) {
				result.assetParams.assetUnitName = UnitName;
			}

			if (AssetName != null) {
				result.assetParams.assetName = AssetName;
			}

			if (Url != null) {
				result.assetParams.url = Url;
			}

			if (MetaDataHash != null) {
				result.assetParams.metadataHash = MetaDataHash.Bytes;
			}

			if (ManagerAddr != null) {
				result.assetParams.assetManager = ManagerAddr;
			}

			if (ReserveAddr != null) {
				result.assetParams.assetReserve = ReserveAddr;
			}

			if (FreezeAddr != null) {
				result.assetParams.assetFreeze = FreezeAddr;
			}

			if (ClawbackAddr != null) {
				result.assetParams.assetClawback = ClawbackAddr;
			}

			WriteObject(result);
		}
		
	}

}
