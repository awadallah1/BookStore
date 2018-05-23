using BookStore.Domain.Concrete;
using BookStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.WebUI.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        IBooksRepository Repository;
        public AdminController(IBooksRepository repo)
        {
            Repository = repo;
        }
        public ViewResult Index()
        {
            return View(Repository.Books);
        }
[HttpGet]
        public ViewResult Index(string category,string searchvalue)
        {
            var books = Repository.Books;
            if (!string.IsNullOrEmpty(category))
            {
                books = Repository.Books.Where(b => b.Category == category);
            }else books= Repository.Books;
            if (!string.IsNullOrEmpty(searchvalue))
            {
                books = Repository.Books.Where(b => b.Title.ToLower().Contains(searchvalue.ToLower()) ||
                b.Description.ToLower().Contains(searchvalue.ToLower()) || b.Category.ToLower().Contains(searchvalue.ToLower()));
                return View(books.ToList());
            }
            
                var categories = Repository.Books.OrderBy(b => b.Category).Select(b => b.Category).Distinct();
                ViewBag.Category = new SelectList(categories);
            
            
            return View(books.ToList());
           
        }

        public ViewResult Edit(int ISBN)
        {
            Book book = Repository.Books.FirstOrDefault(b => b.ISBN == ISBN);
            return View(book);
        }
        [HttpPost]
        public ActionResult Edit(Book book)
        {
            if (ModelState.IsValid)
            {
                Repository.SaveBook(book);
                if (Request.UrlReferrer.Query.ToString() != string.Empty)
                TempData["message"] = string.Format("({0}) has been saved", book.Title);
                else
                TempData["message"] = string.Format("({0}) has been Created", book.Title);
                return RedirectToAction("index");
            }
            else
            {
                return View(book);
            }
        }
        //

        public ViewResult Create()
        {
            ViewBag.creat = "has been created";
            return View("Edit", new Book());
        }

        [HttpPost]
        public ActionResult Delete(int isbn,string searchvalue)
        {

            Book deletedBook = Repository.DeleteBook(isbn);
            if (deletedBook != null)
            {
                TempData["Message"] = string.Format("({0}) has been Deleted", deletedBook.Title);
            }
            return RedirectToAction("Index",new {searchvalue });
        }
    }
}