using Newtonsoft.Json;

namespace Algorand.PowerShell.Model {

	public class AlgodConfiguration {

		[JsonProperty("host")]
		public string Host { get; set; }

		[JsonProperty("apiKey")]
		public string ApiKey { get; set; }

		[JsonProperty("privateApiKey")]
		public string PrivateApiKey { get; set; }

	}

}
