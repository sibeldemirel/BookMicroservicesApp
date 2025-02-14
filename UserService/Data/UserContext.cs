using UserService.Models;
using Microsoft.EntityFrameworkCore;

namespace BookService.Data
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options) { }
        public DbSet<User> Customers { get; set; }
    }
}
