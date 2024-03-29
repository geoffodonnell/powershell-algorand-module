﻿using Algorand.Algod;
using Algorand.Indexer;
using Newtonsoft.Json;
using System;
using System.Net.Http;

namespace Algorand.PowerShell {

	internal static class PsEnvironment {

		private static readonly string mTokenHeader = "X-Algo-API-Token";
		private static readonly object mLock = new object();

		public static IDefaultApi AlgodDefaultApi { get; set; }

		public static ICommonApi IndexerCommonApi { get; set; }

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
			IndexerCommonApi = new CommonApi(IndexerApiHttpClient);
			IndexerSearchApi = new SearchApi(IndexerApiHttpClient);

			var indexerHost = !String.IsNullOrEmpty(node?.Host)
				? node.Host : null;
			var indexerKey = !String.IsNullOrEmpty(node?.ApiKey)
				? node.ApiKey : String.Empty;

			SetBaseAddress(IndexerApiHttpClient, indexerHost);
			SetApiKey(IndexerApiHttpClient, indexerKey);
		}

		public static void RefreshAlgodApiServiceSettings() {

			// Ideally, I'd be able to change the base URL and api key on the clients
			// but that isn't currently supported, and changing the base URL on HttpClient
			// isn't supported after the first request, so for now just re-initialize
			// everything.

			AlgodDefaultApiHttpClient?.Dispose();
			AlgodPrivateApiHttpClient?.Dispose();
			TryInitializeAlgodClients();
		}

		public static void RefreshIndexerApiServiceSettings() {

			IndexerApiHttpClient?.Dispose();
			TryInitializeIndexerClients();
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
