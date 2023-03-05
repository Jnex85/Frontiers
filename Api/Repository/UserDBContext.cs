using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Repository
{
    public class UserDBContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<University> Universities { get; set; }

        public UserDBContext(DbContextOptions<UserDBContext> options)
            : base(options)
        {

        }
    }
}
