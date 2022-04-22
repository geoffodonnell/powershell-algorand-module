using System;
using System.ComponentModel;
using System.Linq;
using System.Management.Automation;
using SdkAccount = Algorand.Account;

namespace Algorand.PowerShell.Model {

	[TypeConverter(typeof(AccountModelPSTypeConverter))]
	public class AccountModel {

		public string Name { get; set; }

		public string Address => NetworkAccount?.Address?.EncodeAsString();

		public string NetworkGenesisHash { get; set; }

		internal SdkAccount NetworkAccount { get; }

		public AccountModel(SdkAccount account) {
			NetworkAccount = account;
		}

		public virtual SignedTransaction SignTransaction(Transaction transaction) {

			return NetworkAccount.SignTransaction(transaction);
		}

		public virtual string ToMnemonic() {

			return NetworkAccount?.ToMnemonic();
		}

		public override string ToString() {

			return Address;
		}

	}

	public class AccountModelPSTypeConverter : PSTypeConverter {

		public override bool CanConvertFrom(object sourceValue, Type destinationType) {

			return CanConvertTo(sourceValue, destinationType);
		}

		public override bool CanConvertTo(object sourceValue, Type destinationType) {

			if (sourceValue is AccountModel) {
				return destinationType == typeof(string)
					|| destinationType == typeof(Address);
			}

			if (sourceValue is string) {
				return destinationType == typeof(AccountModel);
			}

			return false;
		}

		public override object ConvertFrom(
			object sourceValue, Type destinationType, IFormatProvider formatProvider, bool ignoreCase) {

			return ConvertTo(sourceValue, destinationType, formatProvider, ignoreCase);
		}

		public override object ConvertTo(
			object sourceValue, Type destinationType, IFormatProvider formatProvider, bool ignoreCase) {

			// Not sure if this is neccessary
			if (sourceValue.GetType().IsAssignableFrom(destinationType)) {
				return sourceValue;
			}

			if (sourceValue is AccountModel accountModel) {
				if (destinationType == typeof(string)) {
					return accountModel.Address;
				} else if (destinationType == typeof(Address)) {
					return accountModel.NetworkAccount.Address;
				}
			}

			if (sourceValue is string asString) {

				if (!PsConfiguration.AccountStore.Exists) {
					throw new Exception(
						"AccountStore is not initialized, use Initialize-AlgorandAccountStore and retry this action.");
				}

				if (!PsConfiguration.AccountStore.Opened) {
					throw new Exception(
						"AccountStore is not opened, use Open-AlgorandAccountStore and retry this action.");
				}

				var network = PsConfiguration.GetCurrentNetwork();
				var accounts = PsConfiguration.AccountStore.GetAccounts(network.GenesisHash);
				var result = accounts?
					.FirstOrDefault(s => String.Equals(s.Address, asString, StringComparison.Ordinal));

				if (result == null) {
					result = accounts?
						.FirstOrDefault(s => String.Equals(s.Name, asString, StringComparison.Ordinal));
				}

				return result;
			}

			return null;
		}

	}

}
