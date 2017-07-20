namespace SPARQL_Application.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LongLatToFloat : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BookAuthorDetails", "Latitude", c => c.Single(nullable: false));
            AlterColumn("dbo.BookAuthorDetails", "Longitude", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.BookAuthorDetails", "Longitude", c => c.Double(nullable: false));
            AlterColumn("dbo.BookAuthorDetails", "Latitude", c => c.Double(nullable: false));
        }
    }
}
