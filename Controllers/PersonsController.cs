using Microsoft.AspNetCore.Mvc;

namespace WsCrud.Controllers
{
    [ApiController]
    [Route("api/persons")]
    public class PersonsController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok("Hello, world!"); // placeholder
    }
}