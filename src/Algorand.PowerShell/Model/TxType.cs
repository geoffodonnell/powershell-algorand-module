namespace Algorand.PowerShell.Model {

	public enum TxType {

		Default = 0,

		Payment = 1,

		KeyRegistration = 2,

		AssetConfig = 3,

		AssetTransfer = 4,

		AssetFreeze = 5,

		ApplicationCall = 6
	}

	public static class TxTypeExtensions {

		public static string ToSdkType(this TxType value) {

			return value switch {
				TxType.Default			=> "pay",
				TxType.Payment			=> "pay",
				TxType.KeyRegistration	=> "keyreg",
				TxType.AssetConfig		=> "acfg",
				TxType.AssetTransfer	=> "axfer",
				TxType.AssetFreeze		=> "afrz",
				TxType.ApplicationCall	=> "appl",
				_						=> "pay"
			};
		}

	}

}
