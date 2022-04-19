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

		public static Transaction.Type ToSdkType(this TxType value) {

			switch (value) {
				case TxType.Default:			return Transaction.Type.Default;
				case TxType.Payment:			return Transaction.Type.Payment;
				case TxType.KeyRegistration:	return Transaction.Type.KeyRegistration;
				case TxType.AssetConfig:		return Transaction.Type.AssetConfig;
				case TxType.AssetTransfer:		return Transaction.Type.AssetTransfer;
				case TxType.AssetFreeze:		return Transaction.Type.AssetFreeze;
				case TxType.ApplicationCall:	return Transaction.Type.ApplicationCall;
				default:						return Transaction.Type.Default;
			}
		}

	}

}
