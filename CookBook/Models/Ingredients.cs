using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CookBook.Models
{
    public class Ingredients
    {
        [Key]
        public int IngredientID { get; set; }

        public String IngName { get; set; }


        // An Ingredient can make many recipes
        public ICollection<Recipe> Recipes { get; set; }

    }

    public class IngredientsDto
    {
        public int IngredientID { get; set; }
        public string IngName { get; set; }
    }
}