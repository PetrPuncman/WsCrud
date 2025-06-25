using WsCrud.Models;
using WsCrud.Interfaces;

namespace WsCrud.Repositories
{
	/// <summary>
	/// An in-memory repository storing <see cref="Person"/> objects in a thread-safe list.
	/// Suitable for testing and development purposes.
	/// </summary>
	public class InMemoryPersonRepository : IRepository<Person>
	{
		/// <summary>
		/// Internal list storing all Person records.
		/// </summary>
		private readonly List<Person> _people = new();

		/// <summary>
		/// Tracks the next available unique identifier for a new Person.
		/// </summary>
		private int _nextId = 1;

		/// <summary>
		/// Synchronization object for thread safety.
		/// </summary>
		private readonly object _lock = new();

		/// <summary>
		/// Retrieves all people from the repository.
		/// </summary>
		/// <returns>A task that returns a new list of all Person objects.</returns>
		public Task<List<Person>> GetAllAsync()
		{
			lock (_lock)
			{
				return Task.FromResult(new List<Person>(_people));
			}
		}

		/// <summary>
		/// Retrieves a person by their ID.
		/// </summary>
		/// <param name="id">The unique identifier of the person.</param>
		/// <returns>A task that returns the matching Person, or null if not found.</returns>
		public Task<Person?> GetByIdAsync(int id)
		{
			lock (_lock)
			{
				return Task.FromResult(_people.FirstOrDefault(p => p.Id == id));
			}
		}

		/// <summary>
		/// Adds a new person to the repository and assigns a unique ID.
		/// </summary>
		/// <param name="person">The person object to add. The ID will be overwritten.</param>
		/// <returns>A completed task.</returns>
		public Task AddAsync(Person person)
		{
			lock (_lock)
			{
				person.Id = _nextId++;
				_people.Add(person);
				return Task.CompletedTask;
			}
		}

		/// <summary>
		/// Updates an existing person in the repository.
		/// </summary>
		/// <param name="person">The updated person. The person is matched by ID.</param>
		/// <returns>A completed task.</returns>
		public Task UpdateAsync(Person person)
		{
			lock (_lock)
			{
				var index = _people.FindIndex(p => p.Id == person.Id);
				if (index >= 0)
					_people[index] = person;
				return Task.CompletedTask;
			}
		}

		/// <summary>
		/// Deletes a person from the repository by ID.
		/// </summary>
		/// <param name="id">The unique identifier of the person to remove.</param>
		/// <returns>A completed task.</returns>
		public Task DeleteAsync(int id)
		{
			lock (_lock)
			{
				_people.RemoveAll(p => p.Id == id);
				return Task.CompletedTask;
			}
		}
	}
}
