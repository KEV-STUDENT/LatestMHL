using MHL_DB_Model;
using Microsoft.EntityFrameworkCore;

namespace MHL_DB_SQLite
{
    public class DBModelSQLite : DBModel
    {
        private readonly string dBFile;

        public DBModelSQLite(string dBFile):base()
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite( string.Concat("Data Source=", dBFile) );
        }
    }
}