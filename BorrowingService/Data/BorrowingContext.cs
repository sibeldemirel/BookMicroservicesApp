using BorrowingService.Models;
using Microsoft.EntityFrameworkCore;

namespace BookService.Data
{
    public class BookContext : DbContext
    {
        public BookContext(DbContextOptions<BookContext> options) : base(options) { }
        public DbSet<Borrowing> Customers { get; set; }
    }
}
