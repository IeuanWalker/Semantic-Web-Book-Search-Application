using System.Data.Entity;

namespace SPARQL_Application.Models
{
    public class BookDbContext : DbContext
    {
        public DbSet<BookUserSearch> BookNameTable { get; set; }
        public DbSet<BookDetails> BookDetailsTable { get; set; }
        public DbSet<BookAuthorDetails> AuthorDetailsTable { get; set; }
    }
}