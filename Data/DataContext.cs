using Microsoft.EntityFrameworkCore;


namespace WebApiFarm
{
    public class DataContext : DbContext
     {

      public DataContext(DbContextOptions<DataContext> options): base(options)
      {
        
      }
       public DbSet <Product> Products { get; set; }
       public DbSet <Animal> Animals  {get;set;}
       public DbSet <Stock> Stocks {get; set;}
       public DbSet <Person> Persons {get; set;}
       public DbSet <User> Users {get; set;}
       public DbSet <Token> Tokens {get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Stock>()
            .HasOne(s => s.Product) 
            .WithMany() 
            .HasForeignKey(s => s.ProductId); 
        }

    }


    
}