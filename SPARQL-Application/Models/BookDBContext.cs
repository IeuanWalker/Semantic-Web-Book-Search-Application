using System.Data.Entity;

namespace SPARQL_Application.Models
{
    public class BookDBContext : DbContext
    {
        public DbSet<BookUserSearch> bookNameTable { get; set; }
        public DbSet<BookDetails> bookDetailsTable { get; set; }
        public DbSet<BookAuthorDetails> authorDetailsTable { get; set; }
    }
}