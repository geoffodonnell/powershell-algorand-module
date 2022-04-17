using Algorand.PowerShell.Models;
using Algorand.V2.Algod;
using Algorand.V2.Indexer;
using Newtonsoft.Json;
using System;
using System.Net.Http;

namespace Algorand.PowerShell {

	internal static class Environment {

		private static readonly string mDefaultApiHost = "https://mainnet-api.algonode.cloud";
		private static readonly string mDefaultIndexerHost = "https://mainnet-idx.algonode.cloud";

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

		static Environment() { 

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

			Configuration.Initialize();
			InitializeAlgodClients();
			InitializeIndexerClients();
		}

		private static void InitializeAlgodClients() {

			var config = Configuration.GetModuleConfiguration();

			AlgodDefaultApiHttpClient = new HttpClient();
			AlgodPrivateApiHttpClient = new HttpClient();

			AlgodDefaultApi = new DefaultApi(AlgodDefaultApiHttpClient);
			AlgodCommonApi = new Algorand.V2.Algod.CommonApi(AlgodDefaultApiHttpClient);
			AlgodPrivateApi = new PrivateApi(AlgodPrivateApiHttpClient);

			var algodHost = !String.IsNullOrEmpty(config?.AlgodNode?.Host)
				? config.AlgodNode.Host : mDefaultApiHost;
			var algodKey = !String.IsNullOrEmpty(config?.AlgodNode?.ApiKey)
				? config.AlgodNode.ApiKey : String.Empty;
			var algodPrivateKey = !String.IsNullOrEmpty(config?.AlgodNode?.PrivateApiKey)
				? config.AlgodNode.PrivateApiKey : algodKey;

			SetBaseAddress(AlgodDefaultApiHttpClient, algodHost);
			SetBaseAddress(AlgodPrivateApiHttpClient, algodHost);

			SetApiKey(AlgodDefaultApiHttpClient, algodKey);
			SetApiKey(AlgodPrivateApiHttpClient, algodPrivateKey);
		}

		private static void InitializeIndexerClients() {

			var config = Configuration.GetModuleConfiguration();

			IndexerApiHttpClient = new HttpClient();

			IndexerLookupApi = new LookupApi(IndexerApiHttpClient);
			IndexerCommonApi = new Algorand.V2.Indexer.CommonApi(IndexerApiHttpClient);
			IndexerSearchApi = new SearchApi(IndexerApiHttpClient);

			var indexerHost = !String.IsNullOrEmpty(config?.IndexerNode?.Host)
				? config.IndexerNode.Host : mDefaultIndexerHost;
			var indexerKey = !String.IsNullOrEmpty(config?.IndexerNode?.ApiKey)
				? config.IndexerNode.ApiKey : String.Empty;

			SetBaseAddress(IndexerApiHttpClient, indexerHost);
			SetApiKey(IndexerApiHttpClient, indexerKey);
		}

		public static void SetAlgodNodeConfiguration(string host, string apiKey, string privateApiKey) {

			var config = Configuration.GetModuleConfiguration() ?? new ModuleConfiguration();

			config.AlgodNode = new AlgodConfiguration {
				Host = host,
				ApiKey = apiKey,
			};

			SetBaseAddress(AlgodDefaultApiHttpClient, host);
			SetBaseAddress(AlgodPrivateApiHttpClient, host);

			SetApiKey(AlgodDefaultApiHttpClient, apiKey);

			Configuration.SaveModuleConfiguration(config);
		}

		public static void SetIndexerNodeConfiguration(string host, string apiKey) {

			var config = Configuration.GetModuleConfiguration() ?? new ModuleConfiguration();

			config.IndexerNode = new IndexerConfiguration {
				Host = host,
				ApiKey = apiKey,
			};

			SetBaseAddress(IndexerApiHttpClient, host);
			SetApiKey(IndexerApiHttpClient, apiKey);

			Configuration.SaveModuleConfiguration(config);
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
