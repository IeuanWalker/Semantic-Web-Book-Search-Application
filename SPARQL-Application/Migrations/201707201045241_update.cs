namespace SPARQL_Application.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.BookDetails", new[] { "AuthorID" });
            CreateIndex("dbo.BookDetails", "AuthorId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.BookDetails", new[] { "AuthorId" });
            CreateIndex("dbo.BookDetails", "AuthorID");
        }
    }
}
