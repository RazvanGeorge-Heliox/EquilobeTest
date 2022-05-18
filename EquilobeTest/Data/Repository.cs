namespace Equilobe.Data
{
    public class Repository : IRepository
    {
        private readonly EquilobeDbContext _db;
        public Repository()
        {
            var options = new DbContextOptionsBuilder<EquilobeDbContext>()
               .UseInMemoryDatabase(databaseName: "InMemoryTesting").Options;

            var context = new EquilobeDbContext(options);
            _db = context;
        }

        public IQueryable<Book> Books => _db.Books;
        public IQueryable<Inventory> Inventory => _db.Inventory;

        public void Add<EntityType>(EntityType entity) => _db.Add(entity);
        public void Update<EntityType>(EntityType entity) => _db.Update(entity);
        public void Delete<EntityType>(EntityType entity) => _db.Remove(entity);
        public void SaveChanges() => _db.SaveChanges();

    }
}
