using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apirest.Controller
{
    [ApiController]
    [Route("api/[controller]")]

    public class AccountController :  ControllerBase 
    {
        private readonly ILogger<AccountController> _logger; 
        private readonly DataContext _context;


        public AccountController(ILogger<AccountController> logger, DataContext context)
        {
            _logger = logger;
            _context = context;
        }

            [HttpGet(Name = "GetAccounts")]

            public async Task<ActionResult<IEnumerable<Account>>> GetAccounts()
            {
                return await _context.Accounts.ToListAsync();
            }

            [HttpGet("{id:}", Name = "GetAccount")]
            public async  Task<ActionResult<Account>> GetAccount(int id)
            {
                var acccount = await _context.Accounts.FindAsync(id);

                if (acccount == null)
                {
                        return NotFound();
                }

                return acccount;

            }

            [HttpPost]
            public async Task<ActionResult<Account>> Post(Account account)
            {
                _context.Add(account);
                await _context.SaveChangesAsync();

                return new CreatedAtRouteResult("GetAccounts", new {id = account.Id}, account);
            }
            [HttpPut("{id}")]

            public async Task<ActionResult> Put(int id, Account account)
            {
                if(id != account.Id)
                {
                        return BadRequest();
                }

                _context.Entry(account).State = EntityState.Modified;
                await _context.SaveChangesAsync(); 

                return Ok();    
            }

            
        [HttpDelete("{id}")]

        public async Task <ActionResult<Account>> Delete(int id)
        {
            var account = await _context.Accounts.FindAsync(id);

            if (account == null)
            {
                return NotFound();
            }

            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();

            return account;
        }

    }
    
}