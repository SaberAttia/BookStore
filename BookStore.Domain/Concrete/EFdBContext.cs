using BookStore.Domain.Entities; //19
using System;
using System.Collections.Generic;
using System.Data.Entity; //17
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Concrete //16 After Create Database
{
    public class EFDbContext : DbContext //17,18
    {
        public DbSet<Book> Books { set; get; } //19
    }
}
