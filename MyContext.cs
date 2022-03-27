using Microsoft.EntityFrameworkCore;
using RaFilDaAPI.Entities;

namespace RaFilDaAPI
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options) { }

        public DbSet<Computer> Computers { get; set; }
    }
}