using BookStore.Domain.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.WebUI.Controllers
{
    public class NavController : Controller
    {
        // GET: Nav

        private IBooksRepository repository;

        public NavController(IBooksRepository repo)
        {
            repository = repo;
        }
        public PartialViewResult Menu(string category=null)
        {
            ViewBag.selectedCategory = category;
            IEnumerable<string> categories = repository.Books
                                  .Select( c => c.Category)
                                  .Distinct()
                                  .OrderBy(c => c);
            //string viewName = horizontalLayout ? "MenuHorizontal" : "Menu";
            Dictionary<string, int> counts = new Dictionary<string, int>();
            foreach (var cat in categories)
            {
                int categoryItems = repository.Books.Where(c => c.Category == cat).Count();
                counts.Add(cat, categoryItems);
            }
            return PartialView("FlexMenu",counts);
        }
    }
}