using BookStore.Domain.Concrete;
using BookStore.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.WebUI.Controllers
{
    public class BookController : Controller
    {

        // GET: Book

        private IBooksRepository Repository;
        public int PageSize = 4;
        public BookController(IBooksRepository RepositoryParam)
        {
            this.Repository = RepositoryParam;


        }
        //public ActionResult List()
        //{
        //    return View(Repository.Books);
        //}

        //public ViewResult  List(int page=1)
        //{

        //    return View("List",Repository.Books.
        //        OrderBy(b => b.ISBN).
        //        Skip((page - 1) * PageSize).
        //        Take(PageSize));
        //}

        public ViewResult List(string category, int page = 1)
        {


            BooksListViewModel model = new BooksListViewModel
            {
                Books = Repository.Books
                .Where(b => category==null||b.Category == category)
                .OrderBy(b => b.ISBN)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                ,
                pagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems =category==null?Repository.Books.Count():
                    Repository.Books.Where(b=>b.Category==category).Count()
                }
                ,
                CurrentCategory=category
            };

            return View(model);

        }
    }
}

