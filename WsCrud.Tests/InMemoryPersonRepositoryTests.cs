using System.Collections.Generic;
using System.Threading.Tasks;
using WsCrud.Models;
using WsCrud.Repositories;
using Xunit;

namespace WsCrud.Tests.Repositories
{
	/// <summary>
	/// Unit tests for the <see cref="InMemoryPersonRepository"/> class.
	/// Verifies CRUD behavior for managing <see cref="Person"/> entities in memory.
	/// </summary>
	public class InMemoryPersonRepositoryTests
	{
		/// <summary>
		/// Tests that <see cref="InMemoryPersonRepository.AddAsync"/> assigns an ID 
		/// and successfully adds the new person to the repository.
		/// </summary>
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

		/// <summary>
		/// Tests that <see cref="InMemoryPersonRepository.GetByIdAsync"/> retrieves 
		/// the correct person by ID after insertion.
		/// </summary>
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

		/// <summary>
		/// Tests that <see cref="InMemoryPersonRepository.UpdateAsync"/> properly replaces
		/// a person's data while retaining the same ID.
		/// </summary>
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

		/// <summary>
		/// Tests that <see cref="InMemoryPersonRepository.DeleteAsync"/> 
		/// removes a person by ID from the repository.
		/// </summary>
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

