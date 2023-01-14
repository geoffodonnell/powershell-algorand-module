using Algorand.Algod.Model.Transactions;

namespace Algorand.PowerShell.Model {

	public enum OnComplete : ulong {

		NoOp = 0,

		OptIn = 1,

		CloseOut = 2,

		ClearState = 3,

		UpdateApplication = 4,

		DeleteApplication = 5
	}

	public static class OnCompleteExtensions {

		public static OnCompletion ToSdkType(this OnComplete? value) {

			if (value == null) {
				return OnCompletion.Noop;
			}

			return value switch {
				OnComplete.NoOp					=> OnCompletion.Noop,
				OnComplete.OptIn				=> OnCompletion.Optin,
				OnComplete.CloseOut				=> OnCompletion.Closeout,
				OnComplete.ClearState			=> OnCompletion.Clear,
				OnComplete.UpdateApplication	=> OnCompletion.Update,
				_								=> OnCompletion.Noop
			};
		}

	}

}
