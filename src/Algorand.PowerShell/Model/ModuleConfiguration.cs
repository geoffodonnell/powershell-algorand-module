using Newtonsoft.Json;
using System.Collections.Generic;

namespace Algorand.PowerShell.Model {

	public class ModuleConfiguration {

		[JsonProperty("currentNetwork")]
		public string CurrentNetwork { get; set; }

		[JsonProperty("defaultAccounts")]
		public Dictionary<string, string> DefaultAccounts { get; set; }

		[JsonProperty("networks")]
		public Dictionary<string, NetworkConfiguration> Networks { get; set; }

		public ModuleConfiguration() {

			DefaultAccounts = new Dictionary<string, string>();
			Networks = new Dictionary<string, NetworkConfiguration>();
		}

	}

}
