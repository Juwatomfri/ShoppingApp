using Microsoft.EntityFrameworkCore;
using ShoppingApp.Models;

public class ApplicationContext : DbContext
{
    public DbSet<Shop> Shops { get; set; }
    public DbSet<Product> Products { get; set; }
    public ApplicationContext()
    {
        Database.EnsureDeleted();
        Database.EnsureCreated();
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=LocalDB.db");
    }
}