using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using CookBook.Models;

namespace CookBook.Controllers
{
    public class IngredientsDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all ingredients in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all ingredients in the database
        /// </returns>
        /// <example>
        /// GET: api/IngredientsData/ListIngredients
        /// </example>
        /// 
        [HttpGet]
        public IEnumerable<IngredientsDto> ListIngredients()
        {
            List<Ingredients> ingredients = db.Ingredients.ToList();
            List<IngredientsDto> ingredientsDtos = new List<IngredientsDto>();

            ingredients.ForEach(a => ingredientsDtos.Add(new IngredientsDto()
            {
                IngredientID = a.IngredientID,
                IngName = a.IngName
            })) ; 

            return ingredientsDtos;

        }


        /// <summary>
        /// Returns details about one ingredient in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: details of a particular ingredient
        /// </returns>
        /// <example>
        /// GET: api/IngredientsData/FindIngredient/5
        /// </example>
        ///
        [ResponseType(typeof(Ingredients))]
        [HttpGet]
        public IHttpActionResult FindIngredient(int id)
        {
            Ingredients ingredients = db.Ingredients.Find(id);
            IngredientsDto IngredientsDto = new IngredientsDto()
            {
                IngredientID = ingredients.IngredientID,
                IngName = ingredients.IngName
            };

            if (ingredients == null)
            {
                return NotFound();
            }

            return Ok(IngredientsDto);
        }


        /// <summary>
        /// Updates a particular ingredient in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the Ingredient ID primary key</param>
        /// <param name="ingredient">JSON FORM DATA of an Inggredient</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/IngredientData/UpdateIngredient/5
        /// FORM DATA: Ingredient JSON Object
        /// </example>

        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateIngredient(int id, Ingredients ingredient)
        {
            Debug.WriteLine(ingredient.IngredientID);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ingredient.IngredientID)
            {
                return BadRequest();
            }

            db.Entry(ingredient).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IngredientsExists(id))
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
        /// Add an ingredient in the system 
        /// </summary>
        /// <param name="id">Represents the Ingredient ID primary key</param>
        /// <param name="ingredient">JSON FORM DATA of an Inggredient</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/IngredientData/AddIngredient/
        /// FORM DATA: Ingredient JSON Object
        /// </example>

        [ResponseType(typeof(Ingredients))]
        [HttpPost]
        public IHttpActionResult AddIngredient(Ingredients ingredient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Ingredients.Add(ingredient);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = ingredient.IngredientID }, ingredient);
        }

        /// <summary>
        /// Deletes an ingredient from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the ingredient</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/IngredientData/DeleteIngredient/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Ingredients))]
        [HttpPost]
        public IHttpActionResult DeleteIngredient(int id)
        {
            Ingredients ingredients = db.Ingredients.Find(id);
            if (ingredients == null)
            {
                return NotFound();
            }

            db.Ingredients.Remove(ingredients);
            db.SaveChanges();

            return Ok(ingredients);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool IngredientsExists(int id)
        {
            return db.Ingredients.Count(e => e.IngredientID == id) > 0;
        }
    }
}