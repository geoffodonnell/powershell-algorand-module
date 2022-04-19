using Newtonsoft.Json;

namespace Algorand.PowerShell.Model {

	public class IndexerConfiguration {

		[JsonProperty("host")]
		public string Host { get; set; }

		[JsonProperty("apiKey")]
		public string ApiKey { get; set; }

	}

}
