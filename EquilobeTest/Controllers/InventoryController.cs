using EquilobeTest.Data;
using EquilobeTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EquilobeTest.Controllers
{
    public class InventoryController : Controller
    {
        private readonly IRepository _repository;

        public InventoryController(IRepository repository)
        {
            _repository = repository;
        }

        public int InventoryCount(string ISBN)
        {
            return _repository.Books.Where(x => x.ISBN == ISBN).Select(x => x.Inventory).Count();
        }

        public bool AddInventoryEntry(string ISBN)
        {
            var book = _repository.Books.Where(x => x.ISBN == ISBN).FirstOrDefault();
            if (book != null) 
            { 
                try
                {
                    _repository.Add<Inventory>(new Inventory(true, null, book));
                    _repository.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            else { return false; }
        }

        public bool Rent(int Id)
        {
            var inventoryEntry = _repository.GetItem<Inventory>(Id);
            inventoryEntry.RentingDate = DateTime.Now;
            inventoryEntry.Available = false;
            try
            {
                _repository.Update<Inventory>(inventoryEntry);
                _repository.SaveChanges();
                return true;
            }
            catch(Exception ex){
                return false;
            }
        }

        public double Return(int Id)
        {
            var inventoryEntry = _repository.GetItem<Inventory>(Id);

            TimeSpan daysRented = (TimeSpan)(DateTime.Now - inventoryEntry.RentingDate);
            var numberOfOverdueDays = Math.Ceiling(daysRented.TotalDays) - 14;
            double price = inventoryEntry.Book.DefaultPrice;

            _repository.Update<Inventory>(inventoryEntry);
            _repository.SaveChanges();
            if (numberOfOverdueDays > 0)
            {
                double penaltyPerDay = price * 0.01;
                price = price + (penaltyPerDay * numberOfOverdueDays);
                return price;
            }
            else return price;
        }
    }
}
