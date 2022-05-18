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
    public class BookController : Controller
    {
        private readonly IRepository _repository;

        public BookController(IRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Book> Index()
        {
            return _repository.Books.ToArray();
        }
        
        public bool Create(string name, string iSBN, double defaultPrice)
        {
            try
            {
                _repository.Add<Book>(new Book(name, iSBN, defaultPrice));
                _repository.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
    }
}
