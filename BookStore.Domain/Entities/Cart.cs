using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Entities
{
    public class Cart //99
    {
        private List<CartLine> lineCollection = new  List<CartLine>() ;
        public void AddItem(Book book, int quantity=1) //102
        {
            CartLine line = lineCollection
                                     .Where(b => b.Book.ISBN == book.ISBN)
                                     .FirstOrDefault();
            if (line == null)
            {
                lineCollection.Add(new CartLine { Book = book, Quantity = quantity });
            }
            else line.Quantity += quantity;
        }// End AddItem
        public void RemoveLine(Book book) //103
        {
            lineCollection.RemoveAll(b => b.Book.ISBN == book.ISBN);
        }// End RemoveLine
        public decimal ComputeTotalValue() //104
        {
            return lineCollection.Sum(cl => cl.Book.Price * cl.Quantity);
        }// End ComputeTotalValue
        public void Clear() //105
        {
            lineCollection.Clear();
        }
        public IEnumerable<CartLine> Lines //106
        {
            get { return lineCollection; }
        }
    }//End of Cart class //100
    public class CartLine
    {
        public Book Book { set; get; }
        public int Quantity { set; get; }

    }//End of CartLine //101
}//End of namespace
