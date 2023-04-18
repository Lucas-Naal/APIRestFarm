using Microsoft.EntityFrameworkCore;

namespace apirest
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options ) : base(options)
        {
            
        }
        public DbSet<Account> Accounts {get; set;}
        public DbSet<Animal> Animals {get;set;}
    }
    
}