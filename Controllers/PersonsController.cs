using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WsCrud.Interfaces;
using WsCrud.Models;

namespace WsCrud.Controllers
{
	/// <summary>
	/// Handles all CRUD operations for Person resources.
	/// Requires Basic Authentication for access.
	/// </summary>
	[ApiController]
	[Route("api/persons")]
	[Authorize] // Requires valid BasicAuth credentials for all endpoints
	public class PersonsController : ControllerBase
	{
		private readonly IRepository<Person> _repository;

		/// <summary>
		/// Injects the generic repository for Person entities.
		/// </summary>
		/// <param name="repository">Storage implementation (e.g. JSON, in-memory)</param>
		public PersonsController(IRepository<Person> repository)
		{
			_repository = repository;
		}

		/// <summary>
		/// Retrieves all people in the repository.
		/// </summary>
		/// <returns>List of all Person objects</returns>
		[HttpGet]
		public async Task<ActionResult<List<Person>>> Get()
		{
			var people = await _repository.GetAllAsync();
			return Ok(people);
		}

		/// <summary>
		/// Retrieves a single person by ID.
		/// </summary>
		/// <param name="id">Unique identifier of the person</param>
		/// <returns>Person if found; 404 otherwise</returns>
		[HttpGet("{id}")]
		public async Task<ActionResult<Person>> GetById(int id)
		{
			var person = await _repository.GetByIdAsync(id);
			if (person == null)
				return NotFound(); // Returns 404 if no match
			return Ok(person);
		}

		/// <summary>
		/// Creates a new person.
		/// </summary>
		/// <param name="person">Person data (without ID)</param>
		/// <returns>The created Person with assigned ID</returns>
		[HttpPost]
		public async Task<ActionResult<Person>> Post([FromBody] Person person)
		{
			await _repository.AddAsync(person);
			// Returns 201 with location header for GetById route
			return CreatedAtAction(nameof(GetById), new { id = person.Id }, person);
		}

		/// <summary>
		/// Updates an existing person by ID.
		/// </summary>
		/// <param name="id">ID to match</param>
		/// <param name="person">Updated person object</param>
		/// <returns>204 No Content on success; 400 if ID mismatch</returns>
		[HttpPut("{id}")]
		public async Task<IActionResult> Put(int id, [FromBody] Person person)
		{
			if (id != person.Id)
				return BadRequest("ID in route must match ID in body.");

			await _repository.UpdateAsync(person);
			return NoContent(); // 204 No Content indicates success without body
		}

		/// <summary>
		/// Deletes a person by ID.
		/// </summary>
		/// <param name="id">Person ID to remove</param>
		/// <returns>204 No Content</returns>
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			await _repository.DeleteAsync(id);
			return NoContent(); // 204 No Content indicates success without body
		}
	}
}
