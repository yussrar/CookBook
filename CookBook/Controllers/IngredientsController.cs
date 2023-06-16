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
    public class IngredientsController : Controller
    {
        private static readonly HttpClient client;
        JavaScriptSerializer jss = new JavaScriptSerializer();

        static IngredientsController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44373/api/ingredientsdata/");
        }
        // GET: Ingredients/List
        public ActionResult List()
        {
            //communicate with ingredients data api to retrieve a  list of ingredients

            string url = "listingredients";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<Ingredients> ingredients = response.Content.ReadAsAsync<IEnumerable<Ingredients>>().Result;


            //Debug.WriteLine(response.StatusCode);
            return View(ingredients);
        }

        // GET: Ingredients/Details/5
        public ActionResult Details(int id)

        {
            //objective: communicate with ingredients data api to retrieve a one ingredient

            string url = "findingredient/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            IngredientsDto ingredient = response.Content.ReadAsAsync<IngredientsDto>().Result;

            return View(ingredient);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: ingredients/New
        public ActionResult New()
        {
            return View();
        }

        // POST: ingredients/Create
        [HttpPost]
        public ActionResult Create(Ingredients ingredient)
        {

            //objective: add a new Recipe into our system using API

            string url = "addingredient";

            //converting ingredient object to json object using serializer

            string jsonpayload = jss.Serialize(ingredient);

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

        // GET: ingredients/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "findingredient/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            IngredientsDto selectedingredient = response.Content.ReadAsAsync<IngredientsDto>().Result;
            return View(selectedingredient);
        }

        // POST: ingredients/Update/5
        [HttpPost]
        public ActionResult Update(int id, Ingredients ingredient)
        {
            //objective: Edit an existing ingredient in our system

            string url = "UpdateIngredient/" + id;

            //converting recipe object to json objectusing serializer
            string jsonpayload = jss.Serialize(ingredient);

            //sending json object to url by client
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
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

        // GET: ingredients/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "FindIngredient/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            IngredientsDto selecteding = response.Content.ReadAsAsync<IngredientsDto>().Result;
            return View(selecteding);
        }

        // POST: ingredients/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            //objective: Deleting an existing ingredient in our system

            string url = "deleteingredient/" + id;

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
