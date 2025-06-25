using System.Text.Json;
using WsCrud.Interfaces;
using WsCrud.Models;

namespace WsCrud.Repositories
{
	/// <summary>
	/// JSON-backed implementation of the <see cref="IRepository{Person}"/> interface.
	/// Person records are stored in a single file and serialized/deserialized as JSON.
	/// </summary>
	public class JsonPersonRepository : IRepository<Person>
	{
		/// <summary>
		/// Full file path to the JSON file used for storage.
		/// </summary>
		private readonly string _filePath;

		/// <summary>
		/// Interface for abstract file I/O operations.
		/// </summary>
		private readonly IFileStorage _storage;

		/// <summary>
		/// In-memory list of all persons, synchronized with the JSON file.
		/// </summary>
		private readonly List<Person> _people;

		/// <summary>
		/// Lock used to ensure thread safety on read/write operations.
		/// </summary>
		private static readonly object _lock = new();

		/// <summary>
		/// Initializes a new instance of the <see cref="JsonPersonRepository"/> class.
		/// Loads existing data from the JSON file if it exists.
		/// </summary>
		/// <param name="filePath">The file path to the JSON storage.</param>
		/// <param name="storage">An injected file I/O abstraction.</param>
		public JsonPersonRepository(string filePath, IFileStorage storage)
		{
			_filePath = filePath;
			_storage = storage;
			_people = Load();
		}

		/// <summary>
		/// Loads the person data from the JSON file into memory.
		/// Returns an empty list if the file does not exist or fails to deserialize.
		/// </summary>
		private List<Person> Load()
		{
			lock (_lock)
			{
				if (!_storage.Exists(_filePath))
					return new();

				var json = _storage.Read(_filePath);
				return JsonSerializer.Deserialize<List<Person>>(json) ?? new();
			}
		}

		/// <summary>
		/// Serializes the current in-memory person list to the JSON file.
		/// </summary>
		private void Save()
		{
			lock (_lock)
			{
				var json = JsonSerializer.Serialize(
						_people,
						new JsonSerializerOptions { WriteIndented = true }
				);

				_storage.Write(_filePath, json);
			}
		}

		/// <summary>
		/// Retrieves all persons from memory.
		/// </summary>
		/// <returns>A task that returns a new copy of the person list.</returns>
		public Task<List<Person>> GetAllAsync()
		{
			lock (_lock)
			{
				return Task.FromResult(new List<Person>(_people));
			}
		}

		/// <summary>
		/// Retrieves a person with the specified ID.
		/// </summary>
		/// <param name="id">The unique ID of the person.</param>
		/// <returns>
		/// A task that returns the matching person if found; otherwise, null.
		/// </returns>
		public Task<Person?> GetByIdAsync(int id)
		{
			lock (_lock)
			{
				return Task.FromResult(_people.FirstOrDefault(p => p.Id == id));
			}
		}

		/// <summary>
		/// Adds a new person to the list, assigning them a unique ID.
		/// Also persists the updated list to disk.
		/// </summary>
		/// <param name="person">The new person to add.</param>
		/// <returns>A completed task.</returns>
		public Task AddAsync(Person person)
		{
			lock (_lock)
			{
				person.Id = _people.Count > 0 ? _people.Max(p => p.Id) + 1 : 1;
				_people.Add(person);
				Save();
				return Task.CompletedTask;
			}
		}

		/// <summary>
		/// Updates an existing person by ID. Overwrites the stored object.
		/// </summary>
		/// <param name="person">The updated person object.</param>
		/// <returns>A completed task.</returns>
		public Task UpdateAsync(Person person)
		{
			lock (_lock)
			{
				var index = _people.FindIndex(p => p.Id == person.Id);
				if (index != -1)
				{
					_people[index] = person;
					Save();
				}
				return Task.CompletedTask;
			}
		}

		/// <summary>
		/// Deletes all persons with the specified ID and saves the result.
		/// </summary>
		/// <param name="id">The ID of the person(s) to delete.</param>
		/// <returns>A completed task.</returns>
		public Task DeleteAsync(int id)
		{
			lock (_lock)
			{
				_people.RemoveAll(p => p.Id == id);
				Save();
				return Task.CompletedTask;
			}
		}
	}
}