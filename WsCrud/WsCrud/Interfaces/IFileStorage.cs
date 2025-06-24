namespace WsCrud.Interfaces
{
	/// <summary>
	/// Defines low-level file operations used by repositories and services.
	/// </summary>
	public interface IFileStorage
	{
		/// <summary>
		/// Checks if a file exists at the specified path.
		/// </summary>
		/// <param name="path">File path.</param>
		/// <returns>True if the file exists; otherwise, false.</returns>
		bool Exists(string path);

		/// <summary>
		/// Reads all text from the specified file path.
		/// </summary>
		/// <param name="path">File path.</param>
		/// <returns>Contents of the file as string.</returns>
		string Read(string path);

		/// <summary>
		/// Writes text content to the specified file path.
		/// </summary>
		/// <param name="path">Destination file path.</param>
		/// <param name="content">Text content to write.</param>
		void Write(string path, string content);
	}
}
