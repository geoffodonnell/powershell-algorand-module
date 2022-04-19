﻿using System;
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

			if (sourceValue is AccountModel accountModel) {
				return accountModel.Address;
			}

			if (sourceValue is string asString) {
				var network = PsConfiguration.GetCurrentNetwork();

				// TODO: Find account


			}

			return null;
		}

	}

}