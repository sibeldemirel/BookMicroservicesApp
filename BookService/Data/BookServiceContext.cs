using bookService.Models;
using Microsoft.EntityFrameworkCore;

namespace BookService.Data
{
    public class BookContext : DbContext
    {
        public BookContext(DbContextOptions<BookContext> options) : base(options) { }
        public DbSet<Book> Customers { get; set; }
    }
}
