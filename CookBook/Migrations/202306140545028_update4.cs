namespace CookBook.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update4 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Ingredients", "RecipeID", "dbo.Recipes");
            DropIndex("dbo.Ingredients", new[] { "RecipeID" });
            CreateTable(
                "dbo.IngredientsRecipes",
                c => new
                    {
                        Ingredients_IngredientID = c.Int(nullable: false),
                        Recipe_RecipeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Ingredients_IngredientID, t.Recipe_RecipeId })
                .ForeignKey("dbo.Ingredients", t => t.Ingredients_IngredientID, cascadeDelete: true)
                .ForeignKey("dbo.Recipes", t => t.Recipe_RecipeId, cascadeDelete: true)
                .Index(t => t.Ingredients_IngredientID)
                .Index(t => t.Recipe_RecipeId);
            
            DropColumn("dbo.Ingredients", "IngQuantity");
            DropColumn("dbo.Ingredients", "IngDescription");
            DropColumn("dbo.Ingredients", "RecipeID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Ingredients", "RecipeID", c => c.Int(nullable: false));
            AddColumn("dbo.Ingredients", "IngDescription", c => c.String());
            AddColumn("dbo.Ingredients", "IngQuantity", c => c.String());
            DropForeignKey("dbo.IngredientsRecipes", "Recipe_RecipeId", "dbo.Recipes");
            DropForeignKey("dbo.IngredientsRecipes", "Ingredients_IngredientID", "dbo.Ingredients");
            DropIndex("dbo.IngredientsRecipes", new[] { "Recipe_RecipeId" });
            DropIndex("dbo.IngredientsRecipes", new[] { "Ingredients_IngredientID" });
            DropTable("dbo.IngredientsRecipes");
            CreateIndex("dbo.Ingredients", "RecipeID");
            AddForeignKey("dbo.Ingredients", "RecipeID", "dbo.Recipes", "RecipeId", cascadeDelete: true);
        }
    }
}
