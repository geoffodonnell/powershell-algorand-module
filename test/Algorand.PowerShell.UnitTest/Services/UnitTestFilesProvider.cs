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

			var key = GetKeyFromFullName(fullname);

			mFiles.Remove(key, out var value);
		}

		public virtual bool IsFileExist(string fullname) {

			return mFiles.ContainsKey(
				GetKeyFromFullName(fullname));
		}

		public virtual void MoveFile(string from, string to) {

			var fromKey = GetKeyFromFullName(from);
			var toKey = GetKeyFromFullName(to);

			if (!mFiles.TryGetValue(fromKey, out var value)) {
				throw new FileNotFoundException(fromKey);
			}

			mFiles.TryAdd(toKey, value);
			mFiles.TryRemove(fromKey, out var removed);
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

			var key = GetKeyFromFullName(fullname);
			var bytes = mFiles
				.GetOrAdd(key, s => new byte[mDefaultFileSizeMB * 1024]);

			return new MemoryStream(bytes, writable);
		}

		protected virtual string GetKeyFromFullName(string fullname) {

			if (Path.IsPathRooted(fullname)) {
				return fullname;
			}

			return Path.GetFullPath(
				Path.Combine(Environment.CurrentDirectory, fullname));
		}

	}

}
