using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CookBook.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }

        public String CategoryName { get; set; }

        //A category can have many Recipes
        public ICollection<Recipe> Recipes { get; set; }

    }

    public class CategoryDto
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
    }
}