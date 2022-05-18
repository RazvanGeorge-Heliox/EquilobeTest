namespace Equilobe.Models
{
    public class Book
    {
        public Book()
        {
        }

        public Book(string name, string iSBN, double defaultPrice)
        {
            Name = name;
            ISBN = iSBN;
            DefaultPrice = defaultPrice;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string ISBN { get; set; }
        public double DefaultPrice { get; set; }
        public ICollection<Inventory> Inventory { get; set; }
    }
}
