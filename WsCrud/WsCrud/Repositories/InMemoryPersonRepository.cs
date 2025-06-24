using WsCrud.Models;
using WsCrud.Interfaces;

namespace WsCrud.Repositories
{
    /// <summary>
    /// An in-memory repository storing Person objects in a thread-safe list.
    /// </summary>
    public class InMemoryPersonRepository : IRepository<Person>
    {
        private readonly List<Person> _people = new();
        private int _nextId = 1;
        private readonly object _lock = new();

        public Task<List<Person>> GetAllAsync()
        {
            lock (_lock)
            {
                return Task.FromResult(new List<Person>(_people));
            }
        }

        public Task<Person?> GetByIdAsync(int id)
        {
            lock (_lock)
            {
                return Task.FromResult(_people.FirstOrDefault(p => p.Id == id));
            }
        }

        public Task AddAsync(Person person)
        {
            lock (_lock)
            {
                person.Id = _nextId++;
                _people.Add(person);
                return Task.CompletedTask;
            }
        }

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