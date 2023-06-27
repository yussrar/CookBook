using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CookBook.Models.ViewModels
{
    public class DetailsRecipes
    {
        public RecipeDto Recipe { get; set; }
        public IEnumerable<IngredientsDto> IngredientsUsed { get; set;}

        public IEnumerable<IngredientsDto>  NotAddedIngredients { get; set;}
    }
}