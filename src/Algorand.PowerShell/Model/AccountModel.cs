using System;
using System.Linq;
using System.Management.Automation;

namespace Algorand.PowerShell.Model {

	public class AccountModel {

		public string Name { get; set; }

		public string Address => mAccount.Address.EncodeAsString();

		public string NetworkGenesisHash { get; set; }

		private readonly Algorand.Account mAccount;

		public AccountModel(Algorand.Account account) {
			mAccount = account;
		}

		public virtual SignedTransaction SignTransaction(Transaction transaction) {

			return mAccount.SignTransaction(transaction);
		}

		public virtual string ToMnemonic() {

			return mAccount?.ToMnemonic();
		}

	}

	public class AccountModelPSTypeConverter : PSTypeConverter {

		public override bool CanConvertFrom(object sourceValue, Type destinationType) {

			return CanConvertTo(sourceValue, destinationType);
		}

		public override bool CanConvertTo(object sourceValue, Type destinationType) {

			if (sourceValue is NetworkModel) {
				return destinationType == typeof(string);
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
				return accountModel.Address;
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
