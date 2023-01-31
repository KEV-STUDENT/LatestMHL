using MHL_DB_Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MHL_DB_SQLite
{
    public abstract class DBModel : DbContext
    {
       public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Keyword4Book> Keyword4Books { get; set; }
        public DbSet<Sequence4Book> Sequence4Books { get; set; }
        public DbSet<Volume> Volumes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SetColumns(modelBuilder);
            SetIndexes(modelBuilder);
        }

        protected virtual void SetColumns(ModelBuilder modelBuilder) { }

        protected virtual void SetIndexes(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>().HasIndex(p => new { p.LastName, p.FirstName, p.MiddleName }).IsUnique();
            modelBuilder.Entity<Book>().HasIndex(p => new { p.Path2File, p.EntityInZIP, p.Title }).IsUnique();
            modelBuilder.Entity<Genre>().HasIndex(p => new { p.GenreVal }).IsUnique();
            modelBuilder.Entity<Keyword4Book>().HasIndex(p => new { p.Keyword }).IsUnique();
            modelBuilder.Entity<Sequence4Book>().HasIndex(p => new { p.Name }).IsUnique();
            modelBuilder.Entity<Volume>().HasIndex(p => new { p.Number, p.SequenceId }).IsUnique();
        }
    }
}