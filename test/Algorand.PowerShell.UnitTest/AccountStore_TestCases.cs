using Algorand.PowerShell.Model;
using Algorand.PowerShell.UnitTest.Services;
using KeePassLib.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;

namespace Algorand.PowerShell.UnitTest {

	[TestClass]
	public class AccountStore_TestCases {

		public static readonly string FilePath = $".{Path.DirectorySeparatorChar}accounts.kdbx";
		public const string Password = "password";
		public const string GenesisHashMainnet = "wGHE2Pwdvd7S12BL5FaOP20EGYesN73ktiC1qzkkit8=";
		public const string GenesisHashTestnet = "SGO1GKSzyE7IEPItTxCByw9x8FmnrCDexi9/cOUJOiI=";

		[TestInitialize]
		public void Initialize() {
			IOConnection.m_FilesProvider = new UnitTestFilesProvider();
		}

		[TestCleanup]
		public void Cleanup() {
			
			if (IOConnection.m_FilesProvider is UnitTestFilesProvider provider) {
				provider.Close();
			}
		}

		[TestMethod]
		public void Initialize_AccountStore() {

			var target = new AccountStore(FilePath);

			Assert.IsFalse(target.Exists);
			Assert.IsFalse(target.Opened);

			target.Open(Password);

			Assert.IsTrue(target.Opened);
			Assert.IsTrue(target.Exists);
		}

		[TestMethod]
		public void Add_Account_To_AccountStore() {

			var target = new AccountStore(FilePath);
			var account = new AccountModel(new Algorand.Algod.Model.Account()) {
				Name = "One",
				NetworkGenesisHash = GenesisHashMainnet
			};

			target.Open(Password);

			Assert.IsTrue(target.Opened);

			target.Add(account);

			var accountsMainnet = target.GetAccounts(GenesisHashMainnet);
			var accountsTestnet = target.GetAccounts(GenesisHashTestnet);

			// Adding the account was successful
			Assert.AreEqual(accountsMainnet.Count(), 1);
			Assert.AreEqual(accountsTestnet.Count(), 0);

			// Exact name already exists
			Assert.ThrowsException<Exception>(() => {
				target.Add(new AccountModel(new Algorand.Algod.Model.Account()) {
					Name = "One",
					NetworkGenesisHash = GenesisHashMainnet
				});
			});

			// Case-insensitive name already exists
			Assert.ThrowsException<Exception>(() => {
				target.Add(new AccountModel(new Algorand.Algod.Model.Account()) {
					Name = "ONE",
					NetworkGenesisHash = GenesisHashMainnet
				});
			});

			accountsMainnet = target.GetAccounts(GenesisHashMainnet);
			accountsTestnet = target.GetAccounts(GenesisHashTestnet);

			Assert.AreEqual(accountsMainnet.Count(), 1);
			Assert.AreEqual(accountsTestnet.Count(), 0);

			var mnemonic = account.ToMnemonic();

			// Address already exists
			Assert.ThrowsException<Exception>(() => {
				target.Add(new AccountModel(new Algorand.Algod.Model.Account(mnemonic)) {
					Name = "Two",
					NetworkGenesisHash = GenesisHashMainnet
				});
			});

			accountsMainnet = target.GetAccounts(GenesisHashMainnet);
			accountsTestnet = target.GetAccounts(GenesisHashTestnet);

			Assert.AreEqual(accountsMainnet.Count(), 1);
			Assert.AreEqual(accountsTestnet.Count(), 0);

			// Address exists on different network
			target.Add(new AccountModel(new Algorand.Algod.Model.Account(mnemonic)) {
				Name = "Two",
				NetworkGenesisHash = GenesisHashTestnet
			});

			// Name exists on different network
			target.Add(new AccountModel(new Algorand.Algod.Model.Account()) {
				Name = "One",
				NetworkGenesisHash = GenesisHashTestnet
			});

			accountsMainnet = target.GetAccounts(GenesisHashMainnet);
			accountsTestnet = target.GetAccounts(GenesisHashTestnet);

			Assert.AreEqual(accountsMainnet.Count(), 1);
			Assert.AreEqual(accountsTestnet.Count(), 2);
		}

		[TestMethod]
		public void Remove_Account_From_AccountStore() {

			var target = new AccountStore(FilePath);
			var account = new AccountModel(new Algorand.Algod.Model.Account()) {
				Name = "Two",
				NetworkGenesisHash = GenesisHashMainnet
			};

			target.Open(Password);

			Assert.IsTrue(target.Opened);

			// Add the account
			target.Add(account);

			var accountsMainnet = target.GetAccounts(GenesisHashMainnet);
			var accountsTestnet = target.GetAccounts(GenesisHashTestnet);

			Assert.AreEqual(accountsMainnet.Count(), 1);
			Assert.AreEqual(accountsTestnet.Count(), 0);

			// Remove the account
			target.Remove(account);

			accountsMainnet = target.GetAccounts(GenesisHashMainnet);
			accountsTestnet = target.GetAccounts(GenesisHashTestnet);

			Assert.AreEqual(accountsMainnet.Count(), 0);
			Assert.AreEqual(accountsTestnet.Count(), 0);
		}

	}

}