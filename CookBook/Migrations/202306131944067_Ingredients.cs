namespace CookBook.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Ingredients : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ingredients",
                c => new
                    {
                        IngredientID = c.Int(nullable: false, identity: true),
                        IngName = c.String(),
                        IngDescription = c.String(),
                        RecipeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IngredientID)
                .ForeignKey("dbo.Recipes", t => t.RecipeID, cascadeDelete: true)
                .Index(t => t.RecipeID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ingredients", "RecipeID", "dbo.Recipes");
            DropIndex("dbo.Ingredients", new[] { "RecipeID" });
            DropTable("dbo.Ingredients");
        }
    }
}
