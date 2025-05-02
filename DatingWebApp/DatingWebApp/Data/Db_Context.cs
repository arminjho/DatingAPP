using DatingWebApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatingWebApp.Data
{
    public class Db_Context(DbContextOptions options):DbContext(options)
    {

        public DbSet<AppUser> Users { get; set; }
        


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("Server=127.0.0.1;Database=dating_db1;User=root;Password=2611;", new MySqlServerVersion(new Version(10, 5, 4)));
        }



    }
}
