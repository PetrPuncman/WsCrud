using System.Collections.Generic;
using System.Threading.Tasks;
using WsCrud.Models;
using WsCrud.Repositories;
using Xunit;

namespace WsCrud.Tests.Repositories
{
    public class InMemoryPersonRepositoryTests
    {
        [Fact]
        public async Task AddAsync_AssignsIdAndAddsPerson()
        {
            var repo = new InMemoryPersonRepository();
            var person = new Person { Name = "Alice", Age = 30 };

            await repo.AddAsync(person);
            var all = await repo.GetAllAsync();

            Assert.Single(all);
            Assert.Equal(1, person.Id);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCorrectPerson()
        {
            var repo = new InMemoryPersonRepository();
            var person = new Person { Name = "Bob", Age = 40 };
            await repo.AddAsync(person);

            var retrieved = await repo.GetByIdAsync(person.Id);

            Assert.NotNull(retrieved);
            Assert.Equal("Bob", retrieved!.Name);
        }

        [Fact]
        public async Task UpdateAsync_ReplacesPersonData()
        {
            var repo = new InMemoryPersonRepository();
            var person = new Person { Name = "Charlie", Age = 25 };
            await repo.AddAsync(person);

            person.Name = "Charles";
            await repo.UpdateAsync(person);

            var updated = await repo.GetByIdAsync(person.Id);
            Assert.Equal("Charles", updated?.Name);
        }

        [Fact]
        public async Task DeleteAsync_RemovesPerson()
        {
            var repo = new InMemoryPersonRepository();
            var person = new Person { Name = "Daisy", Age = 22 };
            await repo.AddAsync(person);

            await repo.DeleteAsync(person.Id);
            var all = await repo.GetAllAsync();

            Assert.Empty(all);
        }
    }
}
