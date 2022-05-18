using ConsoleTables;
using Equilobe.Data;

namespace Equilobe.Business
{
    public class BusinessManager
    {
        private readonly IRepository _repository;
         
        public BusinessManager()
        {
            _repository = new Repository();
        }


        //Book Section
        public ConsoleTable GetBooks()
        {
            ConsoleTable bookTable = new ConsoleTable("Name", "ISBN");

            var output = _repository.Books.AsNoTracking();
            foreach(var x in output)
            {
                bookTable.AddRow(x.Name, x.ISBN);
            }
            return bookTable;
        }

        public string CreateBook(string name, string iSBN, double defaultPrice)
        {
            _repository.Add<Book>(new Book(name, iSBN, defaultPrice));
            _repository.SaveChanges();
            return "Success";
        }

        //Inventory Section

        public string InventoryInterogation(string ISBN)
        {
            var x = _repository.Books.Where(x => x.ISBN == ISBN).Select(x => x.Inventory).AsNoTracking().ToArray();
            if (x.Count() != 0 ) { 
                return "For the ISBN " + ISBN + " There where found " + x[0].Count() + " copies inside the inventory, out of which "
                    + x[0].Where(x => x.Available == true).Count() + " are available.";
            }
            else
            {
                return "ISBN not found.";
            }

        }

        public ConsoleTable GetInventory(string ISBN)
        {
            ConsoleTable inventoryTable = new ConsoleTable("Id", "Available", "RentingDate");

            var output = _repository.Books.Where(x => x.ISBN == ISBN).Select(x => x.Inventory).AsNoTracking().ToArray();
            foreach (var x in output[0])
            {
                inventoryTable.AddRow(x.Id, x.Available, x.RentingDate);
            }
            return inventoryTable;
        }

        public string AddInventoryEntry(string ISBN)
        {
            var book = _repository.Books.Where(x => x.ISBN == ISBN).FirstOrDefault();
            if (book != null) 
            { 
                _repository.Add<Inventory>(new Inventory(true, null, book));
                _repository.SaveChanges();
                return "Success";
 
            }
            else { return "ISBN not found."; }
        }

        public string Rent(int Id)
        {
            var inventoryEntry = _repository.Inventory.Include(x => x.Book).FirstOrDefault(x => x.Id == Id);


            if (inventoryEntry != null)
            {
                if (inventoryEntry.Available == true)
                {
                    inventoryEntry.RentingDate = DateTime.Now;
                    inventoryEntry.Available = false;
                    _repository.Update<Inventory>(inventoryEntry);
                    _repository.SaveChanges();
                    return "The book was rented successfully. The total to pay is: " + inventoryEntry.Book.DefaultPrice + "";
                }
                else return "This specific copy is already rented.";
            }
            else return "A book copy with this Id was not found.";
        }

        public string Return(int Id)
        {
            var inventoryEntry = _repository.Inventory.Include(x => x.Book).FirstOrDefault(x => x.Id == Id);

            if (inventoryEntry != null)
            {
                if (inventoryEntry.Available == false && inventoryEntry.RentingDate != null)
                {
                    TimeSpan daysRented = (TimeSpan)(DateTime.Now - inventoryEntry.RentingDate);
                    var numberOfOverdueDays = Math.Ceiling(daysRented.TotalDays) - 14;

                    inventoryEntry.RentingDate = null;
                    inventoryEntry.Available = true;
                    _repository.Update<Inventory>(inventoryEntry);
                    _repository.SaveChanges();

                    if (numberOfOverdueDays > 0)
                    {
                        double penaltyPerDay = inventoryEntry.Book.DefaultPrice * 0.01;
                        var penalty = penaltyPerDay * numberOfOverdueDays;
                        return "The book will be returned with an extra fee of: " + String.Format("{0:0.00}", penalty) + "";
                    }
                    else return "The book was returned.";
                }
                else return "This specific copy is not rented.";
            }
            else return "A book copy with this Id was not found.";
        }


        // Testing Section
        public void SeedTestData()
        {
            Book seedBook = new Book("SeededUnitTestBook", "Seeded123123", 49.99);
            _repository.Add<Book>(seedBook);
            
            //seed available book copy
            _repository.Add<Inventory>(new Inventory(true, null, seedBook));
            //seed newly rented book copy
            _repository.Add<Inventory>(new Inventory(false, DateTime.Now, seedBook));
            //Seed overdue rented book copy
            _repository.Add<Inventory>(new Inventory(false, new DateTime(2022, 03, 10, 0, 0, 0), seedBook));
            _repository.SaveChanges();
            
            
        }

        public int GetAvailableInventoryId()
        {
            return _repository.Inventory.Where(x => x.Available == true).Select(x => x.Id).FirstOrDefault();
        }
        public int GetOverdueInventoryId()
        {
            return _repository.Inventory.Where(x => x.Available == false && x.RentingDate == new DateTime(2022, 03, 10, 0, 0, 0)).Select(x => x.Id).FirstOrDefault();
        }
        public int GetNotOverdueInventoryId()
        {
            return _repository.Inventory.Where(x => x.Available == false && x.RentingDate != new DateTime(2022, 03, 10, 0, 0, 0)).Select(x => x.Id).FirstOrDefault();
        }

    }
}
