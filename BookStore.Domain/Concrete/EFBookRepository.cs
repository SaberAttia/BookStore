using BookStore.Domain.Abstract; //22
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.Domain.Entities;

namespace BookStore.Domain.Concrete 
{
    public class EFBookRepository : IBookRepository //21,22,23,25
    {
        EFDbContext context = new EFDbContext(); //24
        public IEnumerable<Book> Books //23
        {
            get
            { 
                return context.Books;
            }
        }

        public void SaveBook(Book book) //173 //add, update
        {
            Book dbEntity = context.Books.Find(book.ISBN);
            if(dbEntity == null)
                context.Books.Add(book);
            else
                {
                    dbEntity.Title = book.Title;
                    dbEntity.Specialization = book.Specialization;
                    dbEntity.Price = book.Price;
                    dbEntity.Description = book.Description;
                }
            context.SaveChanges();
        }

        public Book DeleteBook(int ISBN)  //185
        {
            Book dbEntity = context.Books.Find(ISBN);
            if (dbEntity != null)
            { 
                context.Books.Remove(dbEntity);
                context.SaveChanges();
             }
            return dbEntity;
        }
    }
}
