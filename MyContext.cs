using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RaFilDaAPI.Entities;

namespace RaFilDaAPI
{
    public class MyContext : DbContext
    {
        public IConfiguration Configuration { get; }
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
        public DbSet<ReportDetail> ReportDetails { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<BannedSession> BannedSessions { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=mysqlstudenti.litv.sssvt.cz;database=3b1_piterakradek_db1;user=piterakradek;password=123456");
        }
    }
}