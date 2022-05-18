using Equilobe.Business;

namespace Equilobe
{
    class Program
    {
        private static BusinessManager _manager = new BusinessManager();
        static void Main(string[] args)
        {
            _manager.SeedTestData();

            Console.WriteLine("Please select an operation by entering the coresponding number:");
            Console.WriteLine("1 - Add Book");
            Console.WriteLine("2 - Book List");
            Console.WriteLine("3 - Book Availability");
            Console.WriteLine("4 - Rent a book");
            Console.WriteLine("5 - Return a book");
            Console.WriteLine("6 - Add inventory entry");
            Console.WriteLine("7 - Display Inventory for specific book");
            while (true)
            {
                int operation = 0;
                try
                {
                    operation = Int32.Parse(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("Invalid input.");
                    continue;
                }
                switch (operation)
                {
                    case 1:
                        Console.WriteLine("Enter the title:");
                        var title = Console.ReadLine();

                        Console.WriteLine("Enter the ISBN:");
                        var iSBN = Console.ReadLine();

                        Console.WriteLine("Enter the price in a form such as 00.00");
                        double price = 0;

                        try { price = Double.Parse(Console.ReadLine()); }
                        catch { 
                            Console.WriteLine("Price input was of an invalid format.");
                            Console.WriteLine("You may select a new operation.");
                            break;
                        }

                        Console.WriteLine(_manager.CreateBook(title, iSBN, price));
                        Console.WriteLine("You may select a new operation.");
                        break;

                    case 2:
                        _manager.GetBooks().Write();
                        break;

                    case 3:
                        Console.WriteLine("Enter the ISBN:");
                        iSBN = Console.ReadLine();
                        Console.WriteLine(_manager.InventoryInterogation(iSBN));
                        Console.WriteLine("You may select a new operation.");
                        break;

                    case 4:
                        Console.WriteLine("Enter the Id of the specific copy you wish to rent:");
                        var id = Int32.Parse(Console.ReadLine());
                        Console.WriteLine(_manager.Rent(id));
                        Console.WriteLine("You may select a new operation.");
                        break;

                    case 5:
                        Console.WriteLine("Enter the Id of the specific copy you wish to return:");
                        id = Int32.Parse(Console.ReadLine());
                        Console.WriteLine(_manager.Return(id));
                        Console.WriteLine("You may select a new operation.");
                        break;

                    case 6:
                        Console.WriteLine("Enter the ISBN:");
                        iSBN = Console.ReadLine();
                        Console.WriteLine(_manager.AddInventoryEntry(iSBN));
                        Console.WriteLine("You may select a new operation.");
                        break;

                    case 7:
                        Console.WriteLine("Enter the ISBN:");
                        iSBN = Console.ReadLine();
                        _manager.GetInventory(iSBN).Write();
                        Console.WriteLine("You may select a new operation.");
                        break;

                    default:
                        Console.WriteLine("Invalid input.");
                        Console.WriteLine("You may select a new operation.");
                        break;
                }
            }
        }
    }
}