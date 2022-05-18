using Equilobe.Business;

namespace Equilobe.UnitTests
{
    [TestClass]
    public class BookAndInventoryTests
    {
        private static BusinessManager _manager = new BusinessManager();

        [TestInitialize]
        public void Intialize()
        {
            _manager.SeedTestData();
        }


        //CreateBook
        [TestMethod]
        public void CanCreateBook_ReturnsSuccess()
        {
            var result = _manager.CreateBook("UnitTestBook", "f45g4g4534", 49.99);
            Assert.AreEqual(result, "Success");
        }


        //GetBooks
        [TestMethod]
        public void CanGetBooks_ReturnsEntries()
        {
            var result = _manager.GetBooks();
            Assert.IsTrue(result.Rows.Count() != 0);
        }


        //AddInventoryEntry
        [TestMethod]
        public void CanAddInventory_ReturnsSuccess()
        {
            var result = _manager.AddInventoryEntry("Seeded123123");
            Assert.AreEqual(result, "Success");
        }

        [TestMethod]
        public void CanAddInventory_ReturnsISBNNotFound()
        {
            var result = _manager.AddInventoryEntry("NonExistentISBN");
            Assert.AreEqual(result, "ISBN not found.");
        }


        //GetInventory
        [TestMethod]
        public void CanGetInventory_ReturnsEntries()
        {
            var result = _manager.GetInventory("Seeded123123");
            Assert.IsTrue(result.Rows.Count() != 0);
        }


        //InventoryInterogation
        [TestMethod]
        public void CanInterogateInventory_DoesNotReturnNotFound()
        {
            var result = _manager.InventoryInterogation("Seeded123123");
            Assert.IsTrue(result.Contains("For the ISBN"));
        }

        [TestMethod]
        public void CanInterogateInventory_DoesReturnNotFound()
        {
            var result = _manager.InventoryInterogation("NonExistentISBN");
            Assert.AreEqual(result, "ISBN not found.");
        }


        //Rent
        [TestMethod]
        public void CanRentBook_ReturnsSuccessMessage()
        {
            var result = _manager.Rent(_manager.GetAvailableInventoryId());
            Assert.IsTrue(result.Contains("The book was rented successfully."));
        }

        [TestMethod]
        public void CanRentBook_ReturnUnavailableMessage()
        {
            var result = _manager.Rent(_manager.GetNotOverdueInventoryId());
            Assert.IsTrue(result.Contains("This specific copy is already rented."));
        }

        [TestMethod]
        public void CanRentBook_ReturnNotFoundMessage()
        {
            var result = _manager.Rent(99999);
            Assert.IsTrue(result.Contains("A book copy with this Id was not found."));
        }


        //Return
        [TestMethod]
        public void CanReturnBook_ReturnsSuccessMessage()
        {
            var result = _manager.Return(_manager.GetNotOverdueInventoryId());
            Assert.IsTrue(result.Contains("The book was returned."));
        }

        [TestMethod]
        public void CanReturnOverdueBook_ReturnsFeeMessage()
        {
            var result = _manager.Return(_manager.GetOverdueInventoryId());
            Assert.IsTrue(result.Contains("The book will be returned with an extra fee of:"));
        }

        [TestMethod]
        public void CanReturnOverdueBook_ReturnNotRentedMessage()
        {
            var result = _manager.Return(_manager.GetAvailableInventoryId());
            Assert.IsTrue(result.Contains("This specific copy is not rented."));
        }

        [TestMethod]
        public void CanReturnOverdueBook_ReturnNotFoundMessage()
        {
            var result = _manager.Return(999999);
            Assert.IsTrue(result.Contains("A book copy with this Id was not found."));
        }

    }
}