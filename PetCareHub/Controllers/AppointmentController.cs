using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using PassionProject_DentistAppointment.Models;

namespace PassionProject_DentistAppointment.Controllers
{
    public class AppointmentController : Controller
    {
        private static readonly HttpClient client;
        private readonly JavaScriptSerializer jss = new JavaScriptSerializer();

        static AppointmentController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44300/api/");
        }

        // GET: Appointment/List
        public ActionResult List()
        {
            string url = "AppointmentsData/ListAppointment";
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            IEnumerable<AppointmentDto> appointments = response.Content.ReadAsAsync<IEnumerable<AppointmentDto>>().Result;
            Debug.WriteLine("Number of appointments received : ");
            Debug.WriteLine(appointments.Count());

            return View(appointments);
        }

        // GET: Appointment/Details/5
        public ActionResult Details(int id)
        {
            string url = "AppointmentsData/FindAppointment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            AppointmentDto selectedAppointment = response.Content.ReadAsAsync<AppointmentDto>().Result;
            Debug.WriteLine("Appointment received : ");
            Debug.WriteLine(selectedAppointment.AppointmentDate);

            return View(selectedAppointment);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Appointment/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Appointment/Create
        [HttpPost]
        public ActionResult Create(Appointment appointment)
        {
            Debug.WriteLine("the json payload is :");

            string url = "AppointmentsData/AddAppointment";

            string jsonpayload = jss.Serialize(appointment);

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

        // GET: Appointment/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "AppointmentsData/FindAppointment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            AppointmentDto selectedAppointment = response.Content.ReadAsAsync<AppointmentDto>().Result;
            Debug.WriteLine("Appointment received : ");

            return View(selectedAppointment);
        }

        // POST: Appointment/Update/5
        [HttpPost]
        public ActionResult Update(int id, Appointment appointment)
        {
            try
            {
                Debug.WriteLine("The new info is:");
                Debug.WriteLine(appointment.AppointmentID);

                string url = "AppointmentsData/UpdateAppointment/" + id;

                string jsonpayload = jss.Serialize(appointment);
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

        // GET: Appointment/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "AppointmentsData/FindAppointment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            AppointmentDto selectedAppointment = response.Content.ReadAsAsync<AppointmentDto>().Result;
            return View(selectedAppointment);
        }

        // POST: Appointment/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "AppointmentsData/DeleteAppointment/" + id;
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
