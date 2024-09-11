using GestionTurnosApis.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionTurnosApis.Context
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Shift> Shift { get; set; }
        public DbSet<Branch> Branch { get; set; }
        public DbSet<User> User { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }
    }
}
