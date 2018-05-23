using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Entities
{
   public class Cart
    {
        //New Cart to deal with
        List<CartLine> LineCollection = new List<CartLine>();
        //AddItem To Cart
        public void AddItem(Book book, int quantity = 1)
        {
            CartLine line = LineCollection.Where(cl => cl.Book.ISBN == book.ISBN)
                           .FirstOrDefault();
            if (line == null)
            {
                LineCollection.Add(new CartLine { Book = book, Quantity = quantity });
            }
            else
            {
                line.Quantity += quantity;
            }
        }//end AddItem

        //Remove Item From Cart
        public void RemoveItem(Book book)
        {
            LineCollection.RemoveAll(cl => cl.Book.ISBN == book.ISBN);

        }//end Remove Item

        //Compute Total Values of Cart
        public decimal ComputeTotalValue()
        {
            return LineCollection.Sum(cl => cl.Book.Price * cl.Quantity);
        }//End Compute Total Value

        public void clear()
        {
            LineCollection.Clear();
        }//End Clear Cart

        //Return All Cart Details as Property
        public IEnumerable<CartLine> Lines
        {
            get { return LineCollection; }
        }//End of return all lines in cart

    }//End of Cart Class

    //Basic Entity of Cart is cartline to catch book and its quantity
    public class CartLine
    {
        public Book Book { get; set; }
        public int Quantity { get; set; }
    }
}
