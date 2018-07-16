using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; //26
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;  //172

namespace BookStore.Domain.Entities
{
    public class Book //8,10"public"
    {
        [Key] //26
        public int ISBN { set; get; }
        [Required(ErrorMessage ="Please enter a book title")] //176
        public string Title { set; get; }
        [DataType(DataType.MultilineText)] //172
        [Required(ErrorMessage = "Please enter a book description")] //176
        public string Description { set; get; }
        [Required(ErrorMessage = "Please enter a book price")] //176
        [Range(0.1, 9999.99, ErrorMessage = "Please enter a positive price")] //176
        public decimal Price { set; get; }
        [Required]
        public string Specialization { set; get; }
        //public string Author { set; get; }
       
    }
}
