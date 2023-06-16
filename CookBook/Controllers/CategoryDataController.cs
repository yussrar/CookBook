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
    public class CategoryDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all Categories in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Categories in the database
        /// </returns>
        /// <example>
        /// GET: api/CategoryData/ListCategories
        /// </example>
        /// 
        [HttpGet]
        public IEnumerable<CategoryDto> ListCategories()
        {
            List<Category> Categories = db.Category.ToList();
            List<CategoryDto> CategoriesDto = new List<CategoryDto>();

            Categories.ForEach(a => CategoriesDto.Add(new CategoryDto()
            {
                CategoryID = a.CategoryID,
                CategoryName = a.CategoryName,
            }));

            return CategoriesDto;

        }


        /// <summary>
        /// Returns details about one Category in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: details of a particular Category
        /// </returns>
        /// <example>
        /// GET: api/CategoryData/FindCategory/5
        /// </example>
        ///
        [ResponseType(typeof(Category))]
        [HttpGet]
        public IHttpActionResult FindCategory(int id)
        {
            Category category = db.Category.Find(id);
            CategoryDto categoryDto = new CategoryDto()
            {
               CategoryID= category.CategoryID, 
               CategoryName = category.CategoryName,
            };

            if (category == null)
            {
                return NotFound();
            }

            return Ok(categoryDto);
        }


        /// <summary>
        /// Updates a particular Category in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the Ingredient ID primary key</param>
        /// <param name="Category">JSON FORM DATA of a category</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/CategoryData/UpdateCategory/5
        /// FORM DATA: Category JSON Object
        /// </example>

        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateCategory(int id, Category category)
        {
         
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != category.CategoryID)
            {
                return BadRequest();
            }

            db.Entry(category).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
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
        /// Add a Category in the system 
        /// </summary>
        /// <param name="id">Represents the Ingredient ID primary key</param>
        /// <param name="category">JSON FORM DATA of a category</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/CategoryData/AddCategory/
        /// FORM DATA: Category JSON Object
        /// </example>

        [ResponseType(typeof(Ingredients))]
        [HttpPost]
        public IHttpActionResult AddCategory(Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Category.Add(category);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = category.CategoryID }, category);
        }

        /// <summary>
        /// Deletes a category from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the category</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/CategoryData/DeleteCategory/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Ingredients))]
        [HttpPost]
        public IHttpActionResult DeleteCategory(int id)
        {
            Category category = db.Category.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            db.Category.Remove(category);
            db.SaveChanges();

            return Ok(category);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CategoryExists(int id)
        {
            return db.Category.Count(e => e.CategoryID == id) > 0;
        }
    }
}