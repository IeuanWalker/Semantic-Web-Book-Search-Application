namespace SPARQL_Application.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeNameToString : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BookDetails", "Name", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.BookDetails", "Name", c => c.Int(nullable: false));
        }
    }
}
