using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CookBook.Models.ViewModels
{
    public class DetailsIngredients
    {
        public IngredientsDto Ingredients { get; set; }

        public IEnumerable<RecipeDto> InRecipes { get; set; }
    }
}