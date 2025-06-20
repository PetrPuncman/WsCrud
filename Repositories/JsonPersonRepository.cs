using System.Text.Json;
using WsCrud.Interfaces;
using WsCrud.Models;

namespace WsCrud.Repositories
{
    public class JsonPersonRepository : IRepository<Person>
    {
        private readonly string _filePath;
        private readonly IFileStorage _storage;
        private readonly List<Person> _people;
        private static readonly object _lock = new();

        public JsonPersonRepository(string filePath, IFileStorage storage)
        {
            _filePath = filePath;
            _storage = storage;
            _people = Load();
        }

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

        private void Save()
        {
            lock (_lock)
            {
                var json = JsonSerializer.Serialize(_people, new JsonSerializerOptions { WriteIndented = true });
                _storage.Write(_filePath, json);
            }
        }

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
                person.Id = _people.Count > 0 ? _people.Max(p => p.Id) + 1 : 1;
                _people.Add(person);
                Save();
                return Task.CompletedTask;
            }
        }

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
