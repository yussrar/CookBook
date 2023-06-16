using CookBook.Migrations;
using CookBook.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CookBook.Controllers
{
    public class RecipeController : Controller
    {
        private static readonly HttpClient client;
        JavaScriptSerializer jss = new JavaScriptSerializer();

        static RecipeController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44373/api/recipesdata/");
        }
        // GET: Recipe/List
        public ActionResult List()
        {
            //communicate with recipe data api to retrieve a  list of Recipes
            //curl https://localhost:44373/api/recipesdata/listrecipes

            string url = "listrecipes";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<Recipe> recipes = response.Content.ReadAsAsync<IEnumerable<Recipe>>().Result;
            //Debug.WriteLine(recipes.Count());

            //Debug.WriteLine(response.StatusCode);
            return View(recipes);
        }

        // GET: Recipe/Details/5
        public ActionResult Details(int id)

        {    
            //objective: communicate with recipe data api to retrieve a one Recipe
            //curl https://localhost:44373/api/recipesdata/findrecipes/{id}

            string url = "findrecipe/"+id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            RecipeDto recipe = response.Content.ReadAsAsync<RecipeDto>().Result;

            //Debug.WriteLine("recipe details received");
            //Debug.WriteLine(response.StatusCode);
            return View(recipe);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Recipe/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Recipe/Create
        [HttpPost]
        public ActionResult Create(Recipe recipe)
        {
            Debug.WriteLine("the inputted Recipe is :");
            Debug.WriteLine( recipe.RecipeTitle);

            //objective: add a new Recipe into our system using API
            //curl -H "Content-Type:application/json" -d @recipe.json https://localhost:44373/api/recipesdata/addrecipe

            string url = "addrecipe";

            //converting recipe object to json objectusing serializer

            string jsonpayload = jss.Serialize(recipe);

            //Debug.WriteLine("json object added:");
            //Debug.WriteLine(jsonpayload);
            //Debug.WriteLine(url);

            //sending json object to url by client
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            Debug.WriteLine(content);

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }

            else
            {
                return RedirectToAction("Error");
            }
          
        }

        // GET: Recipe/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "findrecipe/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            RecipeDto selectedrecipe = response.Content.ReadAsAsync<RecipeDto>().Result;
            return View(selectedrecipe);
        }

        // POST: Recipe/Update/5
        [HttpPost]
        public ActionResult Update(int id, Recipe recipe)
        {
            //objective: Edit an existing recipe in our system
            //curl -H "Content-Type:application/json" -d @recipe.json https://localhost:44373/api/recipesdata/updaterecipe/5

            string url = "UpdateRecipe/" + id;

            //converting recipe object to json objectusing serializer
            string jsonpayload = jss.Serialize(recipe);

            //sending json object to url by client
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            //Debug.WriteLine(url);
            //Debug.WriteLine(content);
            //Debug.WriteLine("json object updated :");
            //Debug.WriteLine(jsonpayload);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }

            else
            {
                return RedirectToAction("Error");
            }

        }

        // GET: Recipe/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "findrecipe/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            RecipeDto selectedrecipe = response.Content.ReadAsAsync<RecipeDto>().Result;
            return View(selectedrecipe);
        }

        // POST: Recipe/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            //objective: Deleting an existing recipe in our system
            //curl -H "Content-Type:application/json" -d @recipe.json https://localhost:44373/api/recipesdata/deleterecipe/5

            string url = "deleterecipe/" + id;

            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }

            else
            {
                return RedirectToAction("Error");
            }

        }
    }
}
