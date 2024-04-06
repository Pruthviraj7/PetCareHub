using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using PassionProject_DentistAppointment.Models;
using System.Web.Script.Serialization;

namespace PetCareHub.Controllers
{
    public class PetsController : Controller
    {
        private static readonly HttpClient client;
        JavaScriptSerializer jss = new JavaScriptSerializer();
        static PetsController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44300/api/");

        }

        // GET: Pet/List
        public ActionResult List()
        {
            string url = "petsdata/listpets";
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            IEnumerable<PetDto> pets = response.Content.ReadAsAsync<IEnumerable<PetDto>>().Result;
            Debug.WriteLine("Number of pets received : ");
            Debug.WriteLine(pets.Count());

            return View(pets);
        }

        // GET: Pet/Details/5
        public ActionResult Details(int id)
        {
            string url = "PetsData/FindPet/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            PetDto selectedPet = response.Content.ReadAsAsync<PetDto>().Result;
            Debug.WriteLine("Pet received : ");

            return View(selectedPet);
        }


        // GET: Pet/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Pet/Create
        [HttpPost]
        public ActionResult Create(Pet pet)
        {
            string url = "PetsData/AddPet";

            string jsonpayload = jss.Serialize(pet);
            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
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

        // GET: Pet/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "PetsData/FindPet/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            PetDto selectedPet = response.Content.ReadAsAsync<PetDto>().Result;
            Debug.WriteLine("Pet received : ");

            return View(selectedPet);
        }

        // POST: Pet/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Pet pet)
        {
            try
            {
                string url = "PetsData/UpdatePet/" + id;

                string jsonpayload = jss.Serialize(pet);
                Debug.WriteLine(jsonpayload);

                HttpContent content = new StringContent(jsonpayload);
                content.Headers.ContentType.MediaType = "application/json";

                HttpResponseMessage response = client.PostAsync(url, content).Result;

                return RedirectToAction("Details/" + id);
            }
            catch
            {
                return View();
            }
        }

        // GET: Pet/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "PetsData/FindPet/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            PetDto selectedPet = response.Content.ReadAsAsync<PetDto>().Result;
            return View(selectedPet);
        }

        // POST: Pet/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                string url = "PetsData/DeletePet/" + id;
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
            catch
            {
                return RedirectToAction("Error");
            }
        }


        public ActionResult Error()
        {
            return View();
        }
    }
}
