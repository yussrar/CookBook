namespace CookBook.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Recipes", "CookTime", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Recipes", "CookTime", c => c.DateTime(nullable: false));
        }
    }
}
