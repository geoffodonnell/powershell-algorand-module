using Algorand.PowerShell.Models;
using Newtonsoft.Json;
using System.IO;

namespace Algorand.PowerShell {

	internal static class Configuration {

		public static readonly string ConfigurationFolderName = ".algorand";
		public static readonly string ConfigurationFileName = "config.json";
		public static readonly string AccountsDatabaseName = "accounts.kdbx";

		public static AccountStore AccountStore { get; set; }

		public static string ConfigurationDirectory { get; private set; }

		static Configuration() {

			var localAppData = System.Environment.GetFolderPath(
				System.Environment.SpecialFolder.LocalApplicationData);

			ConfigurationDirectory = Path.Combine(localAppData, ConfigurationFolderName);
		}

		internal static void Initialize() {

			EnsureConfigurationDirectoryExists();
			EnsureConfigurationFileExists();
			InitializeAccountStore();
		}

		private static void EnsureConfigurationDirectoryExists() {

			if (!Directory.Exists(ConfigurationDirectory)) { 
				Directory.CreateDirectory(ConfigurationDirectory);
			}
		}

		private static void EnsureConfigurationFileExists() {

			var filePath = Path.Combine(ConfigurationDirectory, ConfigurationFileName);

			if (!File.Exists(filePath)) {
				File.AppendAllText(filePath, "{}");
			}
		}

		private static void InitializeAccountStore() {

			var filePath = Path.Combine(ConfigurationDirectory, AccountsDatabaseName);

			AccountStore = new AccountStore(filePath);
		}

		public static ModuleConfiguration GetModuleConfiguration() {

			var filePath = Path.Combine(ConfigurationDirectory, ConfigurationFileName);
			var serializer = Environment.JsonSerializer;

			using (var file = File.OpenRead(filePath))
			using (var reader = new StreamReader(file))
			using (var json = new JsonTextReader(reader)) {

				return serializer.Deserialize<ModuleConfiguration>(json);
			}
		}

		public static void SaveModuleConfiguration(ModuleConfiguration configuration) {

			var filePath = Path.Combine(ConfigurationDirectory, ConfigurationFileName);
			var serializer = Environment.JsonSerializer;

			using (var file = File.Open(filePath, FileMode.Truncate, FileAccess.ReadWrite))
			using (var writer = new StreamWriter(file))
			using (var json = new JsonTextWriter(writer)) {

				serializer.Serialize(json, configuration);
			}

			return;
		}

	}

}
