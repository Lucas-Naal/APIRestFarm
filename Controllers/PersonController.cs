using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApiFarm.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]

    public class PersonController : ControllerBase
    {
        private readonly ILogger<PersonController> _logger;
       private readonly DataContext _context; 

         public PersonController(ILogger<PersonController> logger, DataContext context)
       {
        _logger = logger;
        _context = context;
       }

       [HttpGet(Name = "GetPersons")]

        public async Task <ActionResult<IEnumerable<Person>>> GetPersons(){
            return await _context.Persons.ToListAsync();
        }

         [HttpGet("{id}", Name = "GetPerson" )]

        public async Task<ActionResult<Person>> GetPerson(int id){

            var person = await _context.Persons.FindAsync(id);

            if (person == null)
            {
                return NotFound();
            }
            
            return person;

        }

          [HttpPost]
        public async Task <ActionResult<Person>> Post(Person person){

            _context.Persons.Add(person);
            await _context.SaveChangesAsync();

            return new CreatedAtRouteResult("GetPersons", new {id = person.Id} , person);             
        }
          [HttpPut("{id}")]

        public async Task <ActionResult> Put(int id, Person person) {

            if (id != person.Id)
            {
                return BadRequest();
            }

            _context.Entry(person).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok();
        }

         [HttpDelete("{id}")]

        public async Task<ActionResult<Person>> Delete(int id) {

            var person = await _context.Persons.FindAsync(id);

            if (person == null)
            {
                return NotFound();
            }

            _context.Persons.Remove(person);
            await _context.SaveChangesAsync();

            return person;
        }

    }
}