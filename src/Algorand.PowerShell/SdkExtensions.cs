using System;
using AlgodException = Algorand.ApiException;
using AlgodExceptionWithMessage = Algorand.ApiException<Algorand.Algod.Model.ErrorResponse>;
using IndexerExceptionWithMessage = Algorand.ApiException<Algorand.Indexer.Model.ErrorResponse>;

namespace Algorand.PowerShell {

	public static class SdkExtensions {

		public static Exception GetExceptionWithBetterMessage(this AlgodException exception) {

			return new Exception(GetMessageFromNode(exception));
		}

		public static string GetMessageFromNode(this AlgodException exception) {

			if (exception == null) {
				return null;
			}

			if (exception is AlgodExceptionWithMessage withErrorResponse1) {

				var result = withErrorResponse1.Result;
				var message = result?.Message;

				return message;
			}

			if (exception is IndexerExceptionWithMessage withErrorResponse2) {

				var result = withErrorResponse2.Result;
				var message = result?.Message;

				return message;
			}

			return null;
		}

	}

}
