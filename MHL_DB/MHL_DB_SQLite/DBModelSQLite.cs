using Microsoft.EntityFrameworkCore;

namespace MHL_DB_SQLite
{
    public class DBModelSQLite : DBModel
    {
        private readonly string dBFile;

        #region [Constructors]
        public DBModelSQLite(string dBFile) : base()
        {
            if (!string.IsNullOrEmpty(dBFile))
            {
                this.dBFile = dBFile;
                Database.EnsureCreated();
            }
            else
            {
                this.dBFile = string.Empty;
            }
        }
        #endregion

        #region [Methods]
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(string.Concat("Data Source=", dBFile));
        }

        protected override void SetColumns(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entity.GetProperties().Where(p => p.ClrType == typeof(string)))
                {
                    property.SetColumnType("TEXT COLLATE NOCASE");
                }
            }
        }
        #endregion
    }
}