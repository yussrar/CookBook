namespace CookBook.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Category_Recipes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryID = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(),
                    })
                .PrimaryKey(t => t.CategoryID);
            
            CreateTable(
                "dbo.RecipeCategories",
                c => new
                    {
                        Recipe_RecipeId = c.Int(nullable: false),
                        Category_CategoryID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Recipe_RecipeId, t.Category_CategoryID })
                .ForeignKey("dbo.Recipes", t => t.Recipe_RecipeId, cascadeDelete: true)
                .ForeignKey("dbo.Categories", t => t.Category_CategoryID, cascadeDelete: true)
                .Index(t => t.Recipe_RecipeId)
                .Index(t => t.Category_CategoryID);
            
            AddColumn("dbo.Ingredients", "IngQuantity", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RecipeCategories", "Category_CategoryID", "dbo.Categories");
            DropForeignKey("dbo.RecipeCategories", "Recipe_RecipeId", "dbo.Recipes");
            DropIndex("dbo.RecipeCategories", new[] { "Category_CategoryID" });
            DropIndex("dbo.RecipeCategories", new[] { "Recipe_RecipeId" });
            DropColumn("dbo.Ingredients", "IngQuantity");
            DropTable("dbo.RecipeCategories");
            DropTable("dbo.Categories");
        }
    }
}
