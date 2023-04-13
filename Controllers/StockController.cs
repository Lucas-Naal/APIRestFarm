using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

 namespace WebApiFarm.Controllers
 {
     [ApiController]
    [Route("/api/[controller]")]

    public class StockController : ControllerBase 
    {
        private readonly ILogger<StockController> _logger;
        private readonly DataContext _context; 

        public StockController(ILogger<StockController> logger, DataContext context)
       {
        _logger = logger;
        _context = context;
       }

       [HttpGet(Name = "GetStockList")]

         public async Task <ActionResult<IEnumerable<Stock>>> GetStockList(){

            var stocks = await _context.Stocks
                .Include(s => s.Product)
                .ToListAsync();

            return stocks;
        }

        [HttpGet("{id}", Name = "GetStock" )]

        public async Task<ActionResult<Stock>> GetStock(int id){

             var stock = await _context.Stocks
                .Include(s => s.Product) 
                .FirstOrDefaultAsync(s => s.Id == id);

            if (stock == null)
            {
                return NotFound();
            }

            return stock;

        }
         [HttpPost]
        public async Task <ActionResult<Stock>> Post(Stock stock){

            _context.Stocks.Add(stock);
            await _context.SaveChangesAsync();

            return new CreatedAtRouteResult("GetProducts", new {id = stock.Id} , stock);             
        }


    }
    
 }