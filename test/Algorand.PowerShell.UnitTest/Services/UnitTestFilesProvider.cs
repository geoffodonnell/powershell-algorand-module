using KeePassLib.Serialization;
using Newtonsoft.Json;
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

			Console.WriteLine(
				$"UnitTestFilesProvider.IsFileExist(): fullName = {fullname}");

			var result = mFiles.ContainsKey(fullname);
			var tryGetResult = mFiles.TryGetValue(fullname, out var value);
			var allKeysJson = JsonConvert.SerializeObject(mFiles.Keys.ToArray());

			Console.WriteLine(
				$"UnitTestFilesProvider.IsFileExist(): result = {result}, tryGetResult = {tryGetResult}");

			Console.WriteLine(
				$"UnitTestFilesProvider.IsFileExist(): allKeysJson = {allKeysJson}");

			return result;
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
