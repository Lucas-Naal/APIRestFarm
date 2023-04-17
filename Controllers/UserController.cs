using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Security.Cryptography;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace WebApiFarm.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly DataContext _context;

        public AuthController(ILogger<AuthController> logger, DataContext context)
        {
            _logger = logger;
            _context = context;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] User user)
        {
           
              var userFromDB = _context.Persons.FirstOrDefault(u => u.Email == user.Email);

    if (userFromDB == null || userFromDB.Password != user.Password)
    {
        return Unauthorized();
    }

    var token = GenerateJwtToken(user.Email, user.Password);

    // guardar el token en la base de datos
    var newToken = new Token
    {
        Value = token,
        Created = DateTime.UtcNow,
        PersonId = userFromDB.Id
    };

    _context.Tokens.Add(newToken);
    _context.SaveChanges();

    return Ok(new { Token = token });
}

private string GenerateJwtToken(string email, string Password)
{
    
   var tokenHandler = new JwtSecurityTokenHandler();

    
    string secretKey = Environment.GetEnvironmentVariable("JWT_SECRET");
   if (string.IsNullOrEmpty(secretKey))
{
    var bytes = new byte[32];
    using (var rng = new RNGCryptoServiceProvider())
    {
        rng.GetBytes(bytes);
    }
    secretKey = Convert.ToBase64String(bytes);
    Environment.SetEnvironmentVariable("JWT_SECRET", secretKey);
}

    var tokenDescriptor = new SecurityTokenDescriptor
    {
        Subject = new ClaimsIdentity(new[]
        {
            new Claim("email", email)
        }),
        Expires = DateTime.UtcNow.AddDays(1),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)), SecurityAlgorithms.HmacSha256Signature)
    };
    var token = tokenHandler.CreateToken(tokenDescriptor);

    return tokenHandler.WriteToken(token);
}

        [HttpGet(Name = "GetPersons")]
          [AllowAnonymous]
        public async Task <ActionResult<IEnumerable<Person>>> GetPersons(){
            return await _context.Persons.ToListAsync();
        }

         [HttpGet("{id}", Name = "GetPerson" )]
             [AllowAnonymous]
        public async Task<ActionResult<Person>> GetPerson(int id){

            var person = await _context.Persons.FindAsync(id);

            if (person == null)
            {
                return NotFound();
            }
            
            return person;

        }

          [HttpPost]
           [AllowAnonymous]
        public async Task <ActionResult<Person>> Post(Person person){

            _context.Persons.Add(person);
            await _context.SaveChangesAsync();

            return new CreatedAtRouteResult("GetPersons", new {id = person.Id} , person);             
        }
          [HttpPut("{id}")]
             [AllowAnonymous]
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
     [AllowAnonymous]
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
