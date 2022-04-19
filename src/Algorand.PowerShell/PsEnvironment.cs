using Algorand.V2.Algod;
using Algorand.V2.Indexer;
using Newtonsoft.Json;
using System;
using System.Net.Http;

namespace Algorand.PowerShell {

	internal static class PsEnvironment {

		private static readonly string mTokenHeader = "X-Algo-API-Token";
		private static readonly object mLock = new object();

		public static Algorand.V2.Algod.ICommonApi AlgodCommonApi { get; set; }

		public static IDefaultApi AlgodDefaultApi { get; set; }

		public static IPrivateApi AlgodPrivateApi { get; set; }

		public static Algorand.V2.Indexer.ICommonApi IndexerCommonApi { get; set; }

		public static ILookupApi IndexerLookupApi { get; set; }

		public static ISearchApi IndexerSearchApi { get; set; }

		public static HttpClient AlgodDefaultApiHttpClient { get; set; }

		public static HttpClient AlgodPrivateApiHttpClient { get; set; }

		public static HttpClient IndexerApiHttpClient { get; set; }

		public static JsonSerializer JsonSerializer { get; private set; }

		private static bool mIsInitialized;

		static PsEnvironment() { 

			mIsInitialized = false;
			JsonSerializer = JsonSerializer.CreateDefault();
		}

		public static void SafeInitialize() {

			if (mIsInitialized) {
				return;
			}

			lock (mLock) {
				if (mIsInitialized) {
					return;
				}

				Initialize();
				mIsInitialized = true;
			}
		}

		private static void Initialize() {

			PsConfiguration.Initialize();

			TryInitializeAlgodClients();
			TryInitializeIndexerClients();
		}

		private static void TryInitializeAlgodClients() {

			try {
				var network = PsConfiguration.GetCurrentNetwork();
				var node = network.AlgodNode;

				AlgodDefaultApiHttpClient = new HttpClient();
				AlgodPrivateApiHttpClient = new HttpClient();

				AlgodDefaultApi = new DefaultApi(AlgodDefaultApiHttpClient);
				AlgodCommonApi = new Algorand.V2.Algod.CommonApi(AlgodDefaultApiHttpClient);
				AlgodPrivateApi = new PrivateApi(AlgodPrivateApiHttpClient);

				var algodHost = !String.IsNullOrEmpty(node?.Host)
					? node.Host : null;
				var algodKey = !String.IsNullOrEmpty(node?.ApiKey)
					? node.ApiKey : String.Empty;
				var algodPrivateKey = !String.IsNullOrEmpty(node?.PrivateApiKey)
					? node.PrivateApiKey : algodKey;

				SetBaseAddress(AlgodDefaultApiHttpClient, algodHost);
				SetBaseAddress(AlgodPrivateApiHttpClient, algodHost);

				SetApiKey(AlgodDefaultApiHttpClient, algodKey);
				SetApiKey(AlgodPrivateApiHttpClient, algodPrivateKey);
			} catch { }
		}

		private static void TryInitializeIndexerClients() {

			var network = PsConfiguration.GetCurrentNetwork();
			var node = network.IndexerNode;

			IndexerApiHttpClient = new HttpClient();

			IndexerLookupApi = new LookupApi(IndexerApiHttpClient);
			IndexerCommonApi = new Algorand.V2.Indexer.CommonApi(IndexerApiHttpClient);
			IndexerSearchApi = new SearchApi(IndexerApiHttpClient);

			var indexerHost = !String.IsNullOrEmpty(node?.Host)
				? node.Host : null;
			var indexerKey = !String.IsNullOrEmpty(node?.ApiKey)
				? node.ApiKey : String.Empty;

			SetBaseAddress(IndexerApiHttpClient, indexerHost);
			SetApiKey(IndexerApiHttpClient, indexerKey);
		}

		public static void RefreshAlgodApiServiceSettings() {

			var network = PsConfiguration.GetCurrentNetwork();

			SetBaseAddress(AlgodDefaultApiHttpClient, network.AlgodNode.Host);
			SetBaseAddress(AlgodPrivateApiHttpClient, network.AlgodNode.Host);

			SetApiKey(AlgodDefaultApiHttpClient, network.AlgodNode.ApiKey);
			SetApiKey(AlgodPrivateApiHttpClient, network.AlgodNode.PrivateApiKey);
		}

		public static void RefreshIndexerApiServiceSettings() {

			var network = PsConfiguration.GetCurrentNetwork();

			SetBaseAddress(IndexerApiHttpClient, network.IndexerNode.Host);
			SetApiKey(IndexerApiHttpClient, network.IndexerNode.ApiKey);
		}

		private static void SetBaseAddress(HttpClient httpClient, string url) {

			httpClient.BaseAddress = new Uri(url);

			if (!httpClient.BaseAddress.IsAbsoluteUri) {
				throw new ArgumentException(
					$"'{nameof(url)}' must be an absolute path.");

			} else if (!httpClient.BaseAddress.AbsolutePath.EndsWith("/")) {

				var builder = new UriBuilder(httpClient.BaseAddress) {
					Path = httpClient.BaseAddress.AbsolutePath + "/"
				};

				httpClient.BaseAddress = builder.Uri;
			}
		}

		private static void SetApiKey(HttpClient httpClient, string apiKey) {

			if (httpClient.DefaultRequestHeaders.Contains(mTokenHeader)) {
				httpClient.DefaultRequestHeaders.Remove(mTokenHeader);
			}

			if (String.IsNullOrEmpty(apiKey)) {
				return;
			}

			httpClient.DefaultRequestHeaders
				.TryAddWithoutValidation(mTokenHeader, apiKey);
		}

	}

}
