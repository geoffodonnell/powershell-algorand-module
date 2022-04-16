using Algorand.V2.Indexer.Model;

namespace Algorand.PowerShell.Models {

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

			switch (value) {
				case OnComplete.NoOp:				return OnCompletion.Noop;
				case OnComplete.OptIn:				return OnCompletion.Optin;
				case OnComplete.CloseOut:			return OnCompletion.Closeout;
				case OnComplete.ClearState:			return OnCompletion.Clear;
				case OnComplete.UpdateApplication:	return OnCompletion.Update;
				default:							return OnCompletion.Noop;
			}
		}

	}

}
