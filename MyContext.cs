using Microsoft.EntityFrameworkCore;
using RaFilDaAPI.Entities;

namespace RaFilDaAPI
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options) { }

        public DbSet<Computer> Computers { get; set; }
        public DbSet<Config> Configs { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<CompGroup> CompGroups { get; set; }
        public DbSet<CompConf> CompConfs { get; set; }
        public DbSet<ConfGroup> ConfGroups { get; set; }
        public DbSet<Destination> Destination { get; set; }
        public DbSet<Source> Source { get; set; }
    }
}