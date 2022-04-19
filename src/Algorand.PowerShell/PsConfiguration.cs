using Algorand.PowerShell.Model;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;

namespace Algorand.PowerShell {

	internal static class PsConfiguration {

		public static readonly string ConfigurationFolderName = ".algorand";
		public static readonly string ConfigurationFileName = "config.json";
		public static readonly string AccountsDatabaseName = "accounts.kdbx";

		public static AccountStore AccountStore { get; set; }

		public static ModuleConfiguration ModuleConfiguration { get; set; }

		public static string ConfigurationDirectory { get; private set; }

		static PsConfiguration() {

			var localAppData = System.Environment.GetFolderPath(
				System.Environment.SpecialFolder.LocalApplicationData);

			ConfigurationDirectory = Path.Combine(localAppData, ConfigurationFolderName);
		}

		public static NetworkConfiguration[] GetNetworks() {

			return ModuleConfiguration.Networks.Values.ToArray();
		}

		public static NetworkConfiguration GetNetworkOrThrow(string nameOrGenesisHash) {

			var config = ModuleConfiguration;
			var result = config
				.Networks
				.Values
				.FirstOrDefault(s => String.Equals(s.Name, nameOrGenesisHash, StringComparison.InvariantCultureIgnoreCase));

			if (result != null) {
				return result;
			}

			if (config.Networks.TryGetValue(nameOrGenesisHash, out result)) {
				return result;
			}

			throw new Exception(
				$"Network not found. {nameOrGenesisHash} does not match any configuration network.");
		}

		public static void UpsertNetwork(NetworkConfiguration value) {

			if (String.IsNullOrWhiteSpace(value?.Name)) {
				throw new ArgumentNullException(nameof(value.Name));
			}

			if (String.IsNullOrWhiteSpace(value?.GenesisId)) {
				throw new ArgumentNullException(nameof(value.GenesisId));
			}

			if (String.IsNullOrWhiteSpace(value?.GenesisHash)) {
				throw new ArgumentNullException(nameof(value.GenesisHash));
			}

			ModuleConfiguration.Networks[value.GenesisHash] = value;

			SaveModuleConfiguration(ModuleConfiguration);
		}

		public static void RemoveNetwork(string nameOrGenesisHash) {

			var value = GetNetworkOrThrow(nameOrGenesisHash);

			ModuleConfiguration.Networks.Remove(value.GenesisHash);

			SaveModuleConfiguration(ModuleConfiguration);
		}

		public static NetworkConfiguration GetCurrentNetwork() {

			return GetNetworkOrThrow(ModuleConfiguration.CurrentNetwork);
		}

		public static void SetCurrentNetwork(string nameOrGenesisHash) {

			var value = GetNetworkOrThrow(nameOrGenesisHash);

			ModuleConfiguration.CurrentNetwork = value?.GenesisHash;

			SaveModuleConfiguration(ModuleConfiguration);
		}

		public static string GetDefaultAccount(string nameOrGenesisHash) {

			var value = GetNetworkOrThrow(nameOrGenesisHash);

			return ModuleConfiguration.DefaultAccounts[value.GenesisHash];
		}

		public static void SetDefaultAccount(
			string nameOrGenesisHash, string address) {

			var value = GetNetworkOrThrow(nameOrGenesisHash);

			ModuleConfiguration.DefaultAccounts[value.GenesisHash] = address;

			SaveModuleConfiguration(ModuleConfiguration);
		}

		internal static void Initialize() {

			EnsureConfigurationDirectoryExists();
			EnsureConfigurationFileExists();
			InitializeAccountStore();
			InitializeModuleConfiguration();
		}

		private static void EnsureConfigurationDirectoryExists() {

			if (!Directory.Exists(ConfigurationDirectory)) { 
				Directory.CreateDirectory(ConfigurationDirectory);
			}
		}

		private static void EnsureConfigurationFileExists() {

			var filePath = Path.Combine(ConfigurationDirectory, ConfigurationFileName);

			if (!File.Exists(filePath)) {
				var defaultConfiguration = PsConstant.DefaultModuleConfiguration;

				File.AppendAllText(
					filePath, JsonConvert.SerializeObject(defaultConfiguration));
			}
		}

		private static void InitializeAccountStore() {

			var filePath = Path.Combine(ConfigurationDirectory, AccountsDatabaseName);

			AccountStore = new AccountStore(filePath);
		}

		private static void InitializeModuleConfiguration() {

			var filePath = Path.Combine(ConfigurationDirectory, ConfigurationFileName);
			var serializer = PsEnvironment.JsonSerializer;

			using (var file = File.OpenRead(filePath))
			using (var reader = new StreamReader(file))
			using (var json = new JsonTextReader(reader)) {

				ModuleConfiguration = serializer.Deserialize<ModuleConfiguration>(json);
			}
		}

		private static void SaveModuleConfiguration(ModuleConfiguration configuration) {

			var filePath = Path.Combine(ConfigurationDirectory, ConfigurationFileName);
			var serializer = PsEnvironment.JsonSerializer;

			using (var file = File.Open(filePath, FileMode.Truncate, FileAccess.ReadWrite))
			using (var writer = new StreamWriter(file))
			using (var json = new JsonTextWriter(writer)) {

				serializer.Serialize(json, configuration);
			}

			return;
		}

	}

}
