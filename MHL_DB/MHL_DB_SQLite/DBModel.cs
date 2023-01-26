using MHL_DB_Model;
using Microsoft.EntityFrameworkCore;

namespace MHL_DB_SQLite
{
    public class DBModel : DbContext
    {
        private readonly string dBFile;

        public DBModel(string dBFile):base()
        {            
            if (!string.IsNullOrEmpty(dBFile))
            {
                this.dBFile = dBFile;
                Database.EnsureCreated();
            }else
            {
                this.dBFile = string.Empty;
            }
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite( string.Concat("Data Source=", dBFile) );
        }
    }
}