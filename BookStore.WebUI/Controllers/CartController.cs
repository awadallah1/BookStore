using BookStore.Domain.Concrete;
using BookStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.WebUI.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        private IBooksRepository Repository;
        private IOrderProcessor orderProcessor;
       
       public CartController(IBooksRepository repo,IOrderProcessor proc)
        {
            Repository = repo;
            orderProcessor = proc;
        }

        public RedirectToRouteResult AddToCart(Cart cart,int isbn,string returnUrl)
        {
            Book book = Repository.Books.FirstOrDefault(b => b.ISBN == isbn);
            if (book!=null)
            {
                cart.AddItem(book, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(Cart cart,int isbn,string returnUrl)
        {
            Book book = Repository.Books.FirstOrDefault(b => b.ISBN == isbn);
            if (book!=null)
            {
                cart.RemoveItem(book);
            }
            return RedirectToAction("Index",new { returnUrl });
        }

        public ViewResult Index(Cart cart,string returnUrl=null)
        {
            CartIndexViewModel CartModel = new CartIndexViewModel { Cart = cart, Returnurl = returnUrl };
            return View(CartModel);
        }

        // Cart Summary
        public PartialViewResult Summary(Cart cart)
        {
            return PartialView(cart);
        }

        // Check Out

        public ViewResult Checkout()
        {
            return View(new ShippingDetails());
        }

        //Check Out Complete Order
        [HttpPost]
        public ViewResult Checkout(Cart cart , ShippingDetails shippingDetails)
        {
            if (cart.Lines.Count()==0)
            {
                ModelState.AddModelError("", "Your Cart Is Empty ,Fill It First Please");
            }
            if (ModelState.IsValid)
            {
                orderProcessor.ProcessOrder(cart, shippingDetails);
                cart.clear();
                
                return View("Completed","",new EmailSettings());
            }
            else return View(shippingDetails);
          
        }

        //I replace it with CartModelBinder of session
        //private Cart getCart()
        //{
        //    Cart cart = (Cart)Session["Cart"];
        //    if (cart==null)
        //    {
        //        cart = new Cart();
        //        Session["Cart"] = cart;
        //    }
        //    return cart;
        //}
    }
}