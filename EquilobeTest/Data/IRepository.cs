namespace Equilobe.Data
{
    public interface IRepository
    {
        public IQueryable<Book> Books { get; }
        public IQueryable<Inventory> Inventory { get; }

        void Add<EntityType>(EntityType entity);
        void Update<EntityType>(EntityType entity);
        void Delete<EntityType>(EntityType entity);

        void SaveChanges();
    }
}
