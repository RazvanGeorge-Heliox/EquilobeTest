namespace Equilobe.Models
{
    public class Inventory
    {
        public Inventory()
        {
        }

        public Inventory(bool available, DateTime? rentingDate, Book book)
        {
            Available = available;
            RentingDate = rentingDate;
            Book = book;
        }

        public int Id { get; set; }
        public bool Available { get; set; }
        public DateTime? RentingDate { get; set; }
        public Book Book { get; set; }
    }
}
