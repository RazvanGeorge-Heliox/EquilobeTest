namespace Equilobe.Data
{
    public class EquilobeDbContext : DbContext
    {

        public EquilobeDbContext(DbContextOptions<EquilobeDbContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Inventory> Inventory { get; set; }
    }
}
