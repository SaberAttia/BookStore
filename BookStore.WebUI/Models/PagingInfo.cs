using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.WebUI.Models
{
    public class PagingInfo //32
    {
        public int TotalItems { set; get; }
        public int ItemsPerPage { set; get; }
        public int CurrentPage { set; get; }
        public int TotalPages
             {
                get { return (int) Math.Ceiling((decimal)TotalItems / ItemsPerPage); }
             }


    }
}