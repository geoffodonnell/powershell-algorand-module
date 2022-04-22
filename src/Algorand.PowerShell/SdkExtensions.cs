using Algorand.V2.Algod.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgodException = Algorand.V2.Algod.Model.ApiException;
using AlgodExceptionWithMessage = Algorand.V2.Algod.Model.ApiException<Algorand.V2.Algod.Model.ErrorResponse>;
using IndexerException = Algorand.V2.Indexer.Model.ApiException;

namespace Algorand.PowerShell {

	public static class SdkExtensions {

		public static Exception GetExceptionWithBetterMessage(this AlgodException exception) {

			return new Exception(GetMessageFromNode(exception));
		}

		public static Exception GetExceptionWithBetterMessage(this IndexerException exception) {

			return new Exception(GetMessageFromNode(exception));
		}

		public static string GetMessageFromNode(this AlgodException exception) {

			if (exception == null) {
				return null;
			}

			if (exception is AlgodExceptionWithMessage withErrorResponse) {

				var result = withErrorResponse.Result;
				var message = result?.Message;

				return message;
			}

			return null;
		}

		public static string GetMessageFromNode(this IndexerException exception) {

			if (exception == null) {
				return null;
			}

			return exception.Response;
		}

	}

}
