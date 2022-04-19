using Newtonsoft.Json;

namespace Algorand.PowerShell.Model {

	public class NetworkConfiguration {

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("genesisId")]
		public string GenesisId { get; set; }

		[JsonProperty("genesisHash")]
		public string GenesisHash { get; set; }

		[JsonProperty("algodNode")]
		public AlgodConfiguration AlgodNode { get; set; }

		[JsonProperty("indexerNode")]
		public IndexerConfiguration IndexerNode { get; set; }

	}

}
