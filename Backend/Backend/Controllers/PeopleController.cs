using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PeopleController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddPerson(Person person)
        {
            try
            {
                _context.People.Add(person);
                await _context.SaveChangesAsync();
                return CreatedAtRoute("GetPerson", new { id = person.Id }, person);
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"An error occurred while adding the person: {ex.Message}");
            }
           
        }

        [HttpGet]
        public async Task<IActionResult> GetPeople()
        {
            try
            {
                var people = await _context.People.ToListAsync();
                return Ok(people);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving people: {ex.Message}");
            }
        }

        [HttpGet("{id}", Name ="GetPerson")] // GET /api/people/1
        public async Task<IActionResult> GetPerson(int id)
        {
            try
            {
                var people = await _context.People.FindAsync(id);


                if(people == null)
                {
                  return NotFound($"Person with ID {id} not found.");
                }

                return Ok(people);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving people: {ex.Message}");
            }
        }


        [HttpPut("{id:int}")] //PUT /api/people/1
        public async Task<IActionResult> UpdatePerson(int id, [FromBody]Person person)
        {
            try
            {
                if(id != person.Id)
                {
                    return BadRequest("ID in the URL does not match ID in the body.");
                }

                var existingPerson = await _context.People.FindAsync(id);
                if (existingPerson == null)
                {
                    return NotFound($"Person with ID {id} not found.");
                }

                existingPerson.FirstName = person.FirstName;
                existingPerson.LastName = person.LastName;

                //_context.People.Update(person);
                await _context.SaveChangesAsync();
                return NoContent(); // 204 status code for successful update without returning content
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"An error occurred while adding the person: {ex.Message}");
            }

        }

        [HttpDelete("{id:int}")] //Delete /api/people/1
        public async Task<IActionResult> DeletePerson(int id)
        {
            try
            {

                var people = await _context.People.FindAsync(id);


                if (people == null)
                {
                    return NotFound($"Person with ID {id} not found.");
                }

                _context.People.Remove(people);
                await _context.SaveChangesAsync();
                return NoContent(); // 204 status code for successful update without returning content
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"An error occurred while adding the person: {ex.Message}");
            }

        }

        //server app: http://localhost:3000
        //client app (js app): http://localhost:5173 => CORS

    }
}
