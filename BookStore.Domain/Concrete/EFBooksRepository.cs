using BookStore.Domain.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.Domain.Entities;

namespace BookStore.Domain.Concrete
{
   public class EFBooksRepository : IBooksRepository
    {
        EFDbContext context = new EFDbContext();
        public IEnumerable<Book> Books
        {

            get {return  context.Books;}
        }

        public Book DeleteBook(int isbn)
        {
            Book dbEntry = context.Books.Find(isbn);
            if (dbEntry != null)
            {
                context.Books.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }

        public void SaveBook(Book book)
        {
            if (book.ISBN==0)
            {
                context.Books.Add(book);
            }
            else
            {
                Book dbEntry = context.Books.Find(book.ISBN);
                //Book dbEntry = context.Books.FirstOrDefault(b => b.ISBN == book.ISBN);
                if (dbEntry!=null)
                {
                    dbEntry.Title = book.Title;
                    dbEntry.Description = book.Description;
                    dbEntry.Price = book.Price;
                    dbEntry.Category = book.Category;
                }
                
            }
            context.SaveChanges();

        }
    }
}
