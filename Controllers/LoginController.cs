using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace apirest.Controller
{
     [ApiController]
    [Route("api/[controller]")]
    
    public class LoginController : ControllerBase 
    {
        private readonly IConfiguration _config;
        private readonly DataContext _context;
        public LoginController(IConfiguration config, DataContext context)
        {
            _config = config;
            _context = context;
        }

         [HttpPost]
         public IActionResult Login(Login user)
         {
            var usuario = Authenticate(user);

            if (usuario != null)
            {
                //crear token }
                var token = Generate(usuario);
                return Ok(new { token = Generate(usuario) });
            }
            return NotFound("Usuario no encontrado");
         }

         private Account Authenticate(Login user)
         {
            var userFromDB = _context.Accounts.FirstOrDefault(u => u.Email == user.Email);
            //var current_user = UserConstants.Accounts.FirstOrDefault(user => user.Email.ToLower() == user.Email.ToLower() && user.Password == user.Password);

            if (userFromDB != null)
            {
                return userFromDB;
            }
            return null; 
         }

         private string Generate(Account user)
         {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new []
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Password),
                new Claim(ClaimTypes.Role, user.Role),
            };

            //crear token
            var token = new JwtSecurityToken
            (
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
         }
    }
}
