using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WsCrud.Interfaces;
using WsCrud.Models;

namespace WsCrud.Controllers
{
    [ApiController]
    [Route("api/persons")]
    [Authorize] // 🔒 Protect all routes
    public class PersonsController : ControllerBase
    {
        private readonly IRepository<Person> _repository;

        public PersonsController(IRepository<Person> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<List<Person>>> Get()
        {
            var people = await _repository.GetAllAsync();
            return Ok(people);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> GetById(int id)
        {
            var person = await _repository.GetByIdAsync(id);
            if (person == null)
                return NotFound();
            return Ok(person);
        }

        [HttpPost]
        public async Task<ActionResult<Person>> Post(Person person)
        {
            await _repository.AddAsync(person);
            return CreatedAtAction(nameof(GetById), new { id = person.Id }, person);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Person person)
        {
            if (id != person.Id)
                return BadRequest();
            await _repository.UpdateAsync(person);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}
