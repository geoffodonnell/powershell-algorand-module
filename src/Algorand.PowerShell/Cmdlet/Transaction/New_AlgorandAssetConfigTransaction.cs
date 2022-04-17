using Algorand.PowerShell.Models;
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
				assetDefaultFrozen = DefaultFrozen.GetValueOrDefault(),
				assetUnitName = UnitName,
				assetName = AssetName,
				url = Url,
				metadataHash = MetaDataHash.Bytes,
				assetManager = ManagerAddr,
				assetReserve = ReserveAddr,
				assetFreeze = FreezeAddr,
				assetClawback = ClawbackAddr
			};

			WriteObject(result);
		}
		
	}

}
