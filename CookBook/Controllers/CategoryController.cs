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
    public class CategoryController : Controller
    {
        private static readonly HttpClient client;
        JavaScriptSerializer jss = new JavaScriptSerializer();

        static CategoryController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44373/api/CategoryData/");
        }
        // GET: Category/List
        public ActionResult List()
        {
            //communicate with category data api to retrieve a  list of categories

            string url = "listcategories";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<Category> categories = response.Content.ReadAsAsync<IEnumerable<Category>>().Result;


            //Debug.WriteLine(response.StatusCode);
            return View(categories);
        }

        // GET: Category/Details/5
        public ActionResult Details(int id)

        {
            //objective: communicate with Category data api to retrieve a one category

            string url = "findCategory/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            CategoryDto category = response.Content.ReadAsAsync<CategoryDto>().Result;

            return View(category);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: category/New
        public ActionResult New()
        {
            return View();
        }

        // POST: category/Create
        [HttpPost]
        public ActionResult Create(Category category)
        {

            //objective: add a new category into our system using API

            string url = "addcategory";

            //converting ingredient object to json object using serializer

            string jsonpayload = jss.Serialize(category);

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

        // GET: category/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "findcategory/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            CategoryDto selectedcategory = response.Content.ReadAsAsync<CategoryDto>().Result;
            return View(selectedcategory);
        }

        // POST: category/Update/5
        [HttpPost]
        public ActionResult Update(int id, Category category)
        {
            //objective: Edit an existing category in our system

            string url = "UpdateCategory/" + id;

            //converting category object to json objectusing serializer
            string jsonpayload = jss.Serialize(category);

            //sending json object to url by client
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine("json object updated :");
            Debug.WriteLine(jsonpayload);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }

            else
            {
                return RedirectToAction("Error");
            }

        }

        // GET: category/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "FindCategory/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            CategoryDto selectedCat = response.Content.ReadAsAsync<CategoryDto>().Result;
            return View(selectedCat);
        }

        // POST: category/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            //objective: Deleting an existing category in our system

            string url = "deletecategory/" + id;

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
