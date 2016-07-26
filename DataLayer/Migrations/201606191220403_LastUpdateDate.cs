namespace DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LastUpdateDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Issues", "LastUpdate", c => c.DateTime(nullable: false, defaultValueSql: "GETUTCDATE()"));
                        
        }
        
        public override void Down()
        {
            DropColumn("dbo.Issues", "LastUpdate");
        }
    }
}
