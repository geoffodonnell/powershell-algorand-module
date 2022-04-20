using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Encoders;
using System;

namespace Algorand.PowerShell.Model {

	public class BytesModel {

		public byte[] Bytes { get; set; }

		public string BytesAsBase64 { get; set; }

		public BytesModel(byte[] bytes) {

			if (bytes == null) {
				throw new NullReferenceException(nameof(bytes));
			}

			Bytes = bytes;
			BytesAsBase64 = Strings.FromUtf8ByteArray(Base64.Encode(bytes));
		}

		public BytesModel(string bytesAsBase64) {

			if (bytesAsBase64 == null) {
				throw new NullReferenceException(nameof(bytesAsBase64));
			}

			Bytes = Base64.Decode(bytesAsBase64);
			BytesAsBase64 = bytesAsBase64;
		}

	}

}
