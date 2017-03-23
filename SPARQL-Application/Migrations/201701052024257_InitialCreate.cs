namespace SPARQL_Application.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BookAuthorDetails",
                c => new
                    {
                        AuthorID = c.Int(nullable: false, identity: true),
                        AuthorLink = c.String(),
                        AuthorName = c.String(),
                        PlaceOfBirthLink = c.String(),
                        PlaceOfBirth = c.String(),
                        Latitude = c.String(),
                        Longitude = c.String(),
                        dataAndTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.AuthorID);
            
            CreateTable(
                "dbo.BookDetails",
                c => new
                    {
                        BookID = c.Int(nullable: false, identity: true),
                        BookLink = c.String(),
                        Name = c.Int(nullable: false),
                        Abstract = c.String(),
                        NumberOfPages = c.Int(nullable: false),
                        Comment = c.String(),
                        AuthorID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BookID)
                .ForeignKey("dbo.BookAuthorDetails", t => t.AuthorID, cascadeDelete: true)
                .Index(t => t.AuthorID);
            
            CreateTable(
                "dbo.BookUserSearches",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SearchedFor = c.String(),
                        Name = c.String(),
                        AuthorLink = c.String(),
                        Author = c.String(),
                        BookLink = c.String(),
                        dataAndTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BookDetails", "AuthorID", "dbo.BookAuthorDetails");
            DropIndex("dbo.BookDetails", new[] { "AuthorID" });
            DropTable("dbo.BookUserSearches");
            DropTable("dbo.BookDetails");
            DropTable("dbo.BookAuthorDetails");
        }
    }
}
