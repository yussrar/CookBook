using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using CookBook.Models;

namespace CookBook.Controllers
{
    public class RecipesDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all Recipes in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all recipes in the database
        /// </returns>
        /// <example>
        /// GET: api/RecipeData/ListRecipes
        /// </example>
        /// 
        [HttpGet]
        public IEnumerable<RecipeDto> ListRecipes()
        {
            List<Recipe> Recipes = db.Recipes.ToList();
            List<RecipeDto> RecipeDtos = new List<RecipeDto>();

            Recipes.ForEach(a => RecipeDtos.Add(new RecipeDto()
            {
                RecipeId = a.RecipeId,
                RecipeTitle = a.RecipeTitle,
                CookTime = a.CookTime,
                ServingSize = a.ServingSize,
                Instructions = a.Instructions,
                
            }));

            return RecipeDtos;
   
        }

        /// <summary>
        /// Returns details about one Recipe in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: details of a particular Recipe
        /// </returns>
        /// <example>
        /// GET: api/RecipeData/FindRecipe/5
        /// </example>
        ///
        [ResponseType(typeof(Recipe))]
        [HttpGet]
        public IHttpActionResult FindRecipe(int id)
        {
            Recipe Recipe = db.Recipes.Find(id);
            RecipeDto RecipeDto = new RecipeDto()
            {
                RecipeId = Recipe.RecipeId,
                RecipeTitle = Recipe.RecipeTitle,
                CookTime = Recipe.CookTime,
                ServingSize = Recipe.ServingSize,
                Instructions = Recipe.Instructions,
            };

            if (Recipe == null)
            {
                return NotFound();
            }

            return Ok(RecipeDto);
        }

        /// <summary>
        /// Updates a particular recipe in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the Ingredient ID primary key</param>
        /// <param name="recipe">JSON FORM DATA of an recipe</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// Post: api/RecipesData/UpdateRecipe/5
        /// FORM DATA: Recipe JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateRecipe(int id, Recipe recipe)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != recipe.RecipeId)
            {
                return BadRequest();
            }

            db.Entry(recipe).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecipeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }



        /// <summary>
        /// Add a recipe in the system 
        /// </summary>
        /// <param name="id">Represents the recipe ID primary key</param>
        /// <param name="ingredient">JSON FORM DATA of an recipe</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/RecipesData/AddRecipe
        /// FORM DATA: Ingredient JSON Object
        /// </example>
        [ResponseType(typeof(Recipe))]
        [HttpPost]
        public IHttpActionResult AddRecipe(Recipe recipe)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Recipes.Add(recipe);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = recipe.RecipeId }, recipe);
        }


        /// <summary>
        /// Deletes a recipe from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the recipe</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// Post: api/RecipesData/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Recipe))]
        [HttpPost]
        public IHttpActionResult DeleteRecipe(int id)
        {
            Recipe recipe = db.Recipes.Find(id);
            if (recipe == null)
            {
                return NotFound();
            }

            db.Recipes.Remove(recipe);
            db.SaveChanges();

            return Ok(recipe);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RecipeExists(int id)
        {
            return db.Recipes.Count(e => e.RecipeId == id) > 0;
        }
    }
}