using KeePassLib;
using KeePassLib.Interfaces;
using KeePassLib.Keys;
using KeePassLib.Security;
using KeePassLib.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Algorand.PowerShell {

	public class AccountStore : IEnumerable<Account> {

		private const string DefaultGroup = "Default";

		public string Location => mFilePath;

		public bool Exists => File.Exists(mFilePath);

		public bool Opened => mDatabase != null;

		private readonly IStatusLogger mStatusLogger;
		private readonly string mFilePath;
		private PwDatabase mDatabase;

		public AccountStore(string filePath) {

			mStatusLogger = new NullStatusLogger();
			mFilePath = filePath;
			mDatabase = null;
		}

		public void Open(string password) {

			if (String.IsNullOrEmpty(password)) {
				throw new ArgumentNullException("password");
			}

			if (mDatabase != null) {
				throw new InvalidOperationException(
					$"AccountStore is already open, call {nameof(Close)} first.");
			}

			try {
				mDatabase = new PwDatabase();

				var connection = IOConnectionInfo.FromPath(mFilePath);
				var kcpPassword = new KcpPassword(password);
				var key = new CompositeKey();
				
				key.AddUserKey(kcpPassword);

				if (Exists) {
					mDatabase.Open(connection, key, mStatusLogger);
				} else {
					mDatabase.New(connection, key);
					mDatabase.Save(mStatusLogger);
					mDatabase.RootGroup.AddGroup(new PwGroup(true, true, DefaultGroup, PwIcon.Folder), true);
					mDatabase.Save(mStatusLogger);
				}
			} catch (Exception) {
				mDatabase = null;
				throw;
			}
		}

		public virtual void Close() {

			mDatabase?.Close();
			mDatabase = null;
		}

		public virtual IEnumerator<Account> GetEnumerator() {
			return ReadAll().GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}

		public virtual void Add(Account account) {

			if (mDatabase == null) {
				return;
			}

			var group = mDatabase
				.RootGroup
				.Groups
				.FirstOrDefault(s => String.Equals(s.Name, DefaultGroup, StringComparison.OrdinalIgnoreCase));

			if (group == null) {
				throw new Exception($"An error occured while retrieving '{DefaultGroup}'");
			}

			foreach (var entry in group.Entries) {

				var title = entry.Strings.ReadSafe("Title")?.Trim();

				if (String.Equals(account.Address.ToString(), title)) {
					return;
				}
			}

			var newEntry = new PwEntry(true, true);

			newEntry.Strings.Set("Title", new ProtectedString(true, account.Address.ToString()));
			newEntry.Strings.Set("Password", new ProtectedString(true, account.ToMnemonic()));

			group.Entries.Add(newEntry);

			mDatabase.Save(mStatusLogger);
		}

		public virtual void Remove(Account account) {

			if (mDatabase == null) {
				return;
			}

			var group = mDatabase
				.RootGroup
				.Groups
				.FirstOrDefault(s => String.Equals(s.Name, DefaultGroup, StringComparison.OrdinalIgnoreCase));

			if (group == null) {
				throw new Exception($"An error occured while retrieving '{DefaultGroup}'");
			}

			var result = new List<Account>();

			foreach (var entry in group.Entries) {

				var title = entry.Strings.ReadSafe("Title")?.Trim();
				var mnemonic = entry.Strings.Get("Password");

				if (String.Equals(account.Address.ToString(), title)) {
					group.Entries.Remove(entry);
					mDatabase.Save(mStatusLogger);
					break;
				}
			}
		}

		protected virtual List<Account> ReadAll() {

			var group = mDatabase?
				.RootGroup
				.Groups
				.FirstOrDefault(s => String.Equals(s.Name, DefaultGroup, StringComparison.OrdinalIgnoreCase));

			if (group == null) {
				return new List<Account>();
			}

			var result = new List<Account>();

			foreach (var entry in group.Entries) {

				var name = entry.Strings.ReadSafe("Title")?.Trim();
				var mnemonic = entry.Strings.Get("Password");

				if (TryCreateAccount(mnemonic, out var account)) {
					result.Add(account);
				}
			}

			return result;
		}

		protected virtual bool TryCreateAccount(ProtectedString mnemonic, out Account account) {

			bool result;

			try {
				account = new Account(
					mnemonic?.ReadString().Trim()?.Replace(",", ""));

				result = true;

#pragma warning disable CS0168 // Variable is declared but never used
			} catch (Exception ex) {
#pragma warning restore CS0168 // Variable is declared but never used
				account = null;
				result = false;
			}

			return result;
		}

	}

}
