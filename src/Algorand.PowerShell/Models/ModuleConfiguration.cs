using Newtonsoft.Json;

namespace Algorand.PowerShell.Models {

	public class ModuleConfiguration {

		[JsonProperty("algodNode")]
		public AlgodConfiguration AlgodNode { get; set; }

		[JsonProperty("indexerNode")]
		public IndexerConfiguration IndexerNode { get; set; }

		[JsonProperty("account")]
		public string? Account { get; set; }

		public ModuleConfiguration() {

			AlgodNode = new AlgodConfiguration();
			IndexerNode = new IndexerConfiguration();
		}

	}

}
