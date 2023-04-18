using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace apirest.Controller
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]

    public class AnimalController :  ControllerBase 
    {
        private readonly ILogger<AnimalController> _logger; 
        private readonly DataContext _context;


        public AnimalController(ILogger<AnimalController> logger, DataContext context)
        {
            _logger = logger;
            _context = context;
        }

            [HttpGet(Name = "GetAnimals")]

            public async Task<ActionResult<IEnumerable<Animal>>> GetAnimals()
            {
                return await _context.Animals.ToListAsync();
            }

            [HttpGet("{id:}", Name = "GetAnimal")]
            public async  Task<ActionResult<Animal>> GetAnimal(int id)
            {
                var animal = await _context.Animals.FindAsync(id);

                if (animal == null)
                {
                        return NotFound();
                }

                return animal;

            }

            [HttpPost]
            public async Task<ActionResult<Animal>> Post(Animal animal)
            {
                _context.Add(animal);
                await _context.SaveChangesAsync();

                return new CreatedAtRouteResult("GetAnimals", new {id = animal.Id}, animal);
            }
            [HttpPut("{id}")]

            public async Task<ActionResult> Put(int id, Animal animal)
            {
                if(id != animal.Id)
                {
                        return BadRequest();
                }

                _context.Entry(animal).State = EntityState.Modified;
                await _context.SaveChangesAsync(); 

                return Ok();    
            }

            
        [HttpDelete("{id}")]

        public async Task <ActionResult<Animal>> Delete(int id)
        {
            var animal = await _context.Animals.FindAsync(id);

            if (animal == null)
            {
                return NotFound();
            }

            _context.Animals.Remove(animal);
            await _context.SaveChangesAsync();

            return animal;
        }

    }
    
}