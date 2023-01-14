using System;

namespace Algorand.PowerShell.Model {

	public enum AddressRole : ulong {

		Unknown = 0,

		Sender = 1,

		Receiver = 2,

		FreezeTarget = 3
	}

	public static class AddressRoleExtensions {

		public static string ToSdkType(this AddressRole? value) {

			if (value == null) {
				return String.Empty;
			}

			return value switch {
				AddressRole.Sender			=> "sender",
				AddressRole.Receiver		=> "receiver",
				AddressRole.FreezeTarget	=> "freeze-target",
				_							=> String.Empty
			};
		}

	}

}
