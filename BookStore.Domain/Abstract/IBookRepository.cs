using BookStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Abstract
{
    public interface IBookRepository //9,10"public"
    {
        IEnumerable<Book> Books { get; }
        void SaveBook(Book book); //173 Add,Edit
        Book DeleteBook(int ISBN); //185 Delete
    }
}
