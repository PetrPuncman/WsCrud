using WsCrud.Interfaces;

namespace WsCrud.Services
{
	/// <summary>
	/// Provides file I/O operations using the actual file system.
	/// </summary>
	public class FileSystemStorage : IFileStorage
	{
		/// <inheritdoc />
		public bool Exists(string path) => File.Exists(path);

		/// <inheritdoc />
		public string Read(string path) => File.ReadAllText(path);

		/// <inheritdoc />
		public void Write(string path, string content) => File.WriteAllText(path, content);
	}
}
