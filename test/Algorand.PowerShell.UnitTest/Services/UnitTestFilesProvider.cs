using KeePassLib.Serialization;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorand.PowerShell.UnitTest.Services {

	internal class UnitTestFilesProvider : IFilesProvider {

		private const ulong mDefaultFileSizeMB = 10;
		private readonly ConcurrentDictionary<string, byte[]> mFiles;

		public UnitTestFilesProvider() {
			mFiles = new ConcurrentDictionary<string, byte[]>(StringComparer.Ordinal);
		}

		public virtual void DeleteFile(string fullname) {

			mFiles.Remove(fullname, out var value);
		}

		public virtual bool IsFileExist(string fullname) {

			return mFiles.ContainsKey(fullname);
		}

		public virtual void MoveFile(string from, string to) {

			if (!mFiles.TryGetValue(from, out var value)) {
				throw new FileNotFoundException(from);
			}

			mFiles.TryAdd(to, value);
			mFiles.TryRemove(from, out var removed);
		}

		public virtual Stream OpenReadLocal(string fullname) {

			return GetFileStream(fullname, false);
		}

		public virtual Stream OpenWriteLocal(string fullname) {

			return GetFileStream(fullname, true);
		}

		internal void Close() {

			mFiles.Clear();
			GC.Collect();
		}

		protected virtual Stream GetFileStream(string fullname, bool writable) {

			var bytes = mFiles
				.GetOrAdd(fullname, s => new byte[mDefaultFileSizeMB * 1024]);

			return new MemoryStream(bytes, writable);
		}

	}

}
