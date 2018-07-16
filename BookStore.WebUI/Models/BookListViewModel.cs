using BookStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.WebUI.Models
{
    public class BookListViewModel //68
    {
        public IEnumerable<Book> Books { set; get; } //69
        public PagingInfo pagingInfo { set; get; } //70
        public string CurrentSpecialization { set; get; } //83
    }
}