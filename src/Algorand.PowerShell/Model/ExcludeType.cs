using System;
using System.Net.NetworkInformation;

namespace Algorand.PowerShell.Model {

	public enum ExcludeType {

		All,
		
		Assets,
		
		CreatedAssets,
		
		AppsLocalState,
		
		CreatedApps,
		
		None

	}

	public static class ExcludeTypeExtensions {

		public static string ToSdkType(this ExcludeType? value) {

			return ToSdkType(value ?? ExcludeType.None);
		}
		
		public static string ToSdkType(this ExcludeType value) {

			return value switch {
				ExcludeType.All => "all",
				ExcludeType.Assets => "assets",
				ExcludeType.CreatedAssets => "created-assets",
				ExcludeType.AppsLocalState => "apps-local-state",
				ExcludeType.CreatedApps => "created-apps",
				ExcludeType.None => "none",
				_ => "none"
			};
		}

	}

}
