using System.Collections.Generic;
using System.Threading.Tasks;

namespace WsCrud.Interfaces
{
	/// <summary>
	/// Generic repository interface for basic CRUD operations.
	/// </summary>
	/// <typeparam name="T">Entity type.</typeparam>
	public interface IRepository<T>
	{
		/// <summary>Gets all entities.</summary>
		Task<List<T>> GetAllAsync();

		/// <summary>Gets a specific entity by ID.</summary>
		Task<T?> GetByIdAsync(int id);

		/// <summary>Adds a new entity.</summary>
		Task AddAsync(T entity);

		/// <summary>Updates an existing entity.</summary>
		Task UpdateAsync(T entity);

		/// <summary>Deletes an entity by ID.</summary>
		Task DeleteAsync(int id);
	}
}