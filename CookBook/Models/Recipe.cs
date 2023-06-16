using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CookBook.Models
{
    public class Recipe
    {
        public int RecipeId { get; set; }
        public string RecipeTitle { get; set; }
        public string Instructions { get; set; }
        public string CookTime { get; set; }
        public int ServingSize { get; set; }

        //A recipe can have many categories
        public ICollection<Category> Category { get; set; }


        //A recipe can have many Ingredients
        public ICollection<Ingredients> Ingredients { get; set; }
    }

    public class RecipeDto
    {
        public int RecipeId { get; set; }
        public string RecipeTitle { get; set; }
        public string Instructions { get; set; }
        public string CookTime { get; set; }
        public int ServingSize { get; set; }
    }
}