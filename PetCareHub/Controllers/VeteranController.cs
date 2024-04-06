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
    public class VeteranController : Controller
    {
        private static readonly HttpClient client;
        JavaScriptSerializer jss = new JavaScriptSerializer();

        static VeteranController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44300/api/");
        }

        // GET: Veteran/List
        public ActionResult List()
        {
            string url = "veterandata/ListVeterans";
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            IEnumerable<VeteranDto> veterans = response.Content.ReadAsAsync<IEnumerable<VeteranDto>>().Result;
            Debug.WriteLine("Number of veterans received: ");
            Debug.WriteLine(veterans.Count());

            return View(veterans);
        }

        // GET: Veteran/Details/5
        public ActionResult Details(int id)
        {
            string veteranUrl = "veterandata/findveteran/" + id;
            HttpResponseMessage veteranResponse = client.GetAsync(veteranUrl).Result;

            Debug.WriteLine("The response code for veteran is ");
            Debug.WriteLine(veteranResponse.StatusCode);

            VeteranDto selectedVeteran = veteranResponse.Content.ReadAsAsync<VeteranDto>().Result;
            Debug.WriteLine("Veteran received: ");
            Debug.WriteLine(selectedVeteran.VeteranName); // Example property for debugging, adjust as needed

            ViewBag.SelectedVeteran = selectedVeteran;

            string bookedAppointmentsUrl = "appointmentsdata/listAppointmentForVeteran/" + id;
            HttpResponseMessage bookedAppointmentsResponse = client.GetAsync(bookedAppointmentsUrl).Result;
            IEnumerable<AppointmentDto> bookedAppointments = bookedAppointmentsResponse.Content.ReadAsAsync<IEnumerable<AppointmentDto>>().Result;

            ViewBag.BookedAppointments = bookedAppointments;

            string availableAppointmentsUrl = "appointmentsdata/listAppointmentNotForVeteran/" + id;
            HttpResponseMessage availableAppointmentsResponse = client.GetAsync(availableAppointmentsUrl).Result;
            IEnumerable<AppointmentDto> availableAppointments = availableAppointmentsResponse.Content.ReadAsAsync<IEnumerable<AppointmentDto>>().Result;

            ViewBag.AvailableAppointments = availableAppointments;

            return View(selectedVeteran);
        }




        [HttpPost]
        public ActionResult Associate(int id, int appointmentId)
        {
            Debug.WriteLine("Attempting to associate veteran: " + id + " with appointment " + appointmentId);

            string url = "appointmentsdata/AssociateAppointmentWithVeteran/" + id + "/" + appointmentId;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }

        [HttpGet]
        public ActionResult UnAssociate(int id, int appointmentId)
        {
            Debug.WriteLine("Attempting to unassociate veteran: " + id + " with appointment: " + appointmentId);

            string url = "appointmentsdata/unassociateappointmentwithveteran/" + id + "/" + appointmentId;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Veteran/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Veteran/Create
        [HttpPost]
        public ActionResult Create(Veteran veteran)
        {
            try
            {
                string url = "veterandata/addveteran";

                // Serialize the veteran object to JSON
                string jsonPayload = jss.Serialize(veteran);
                Debug.WriteLine(jsonPayload);

                // Create the HTTP content with JSON payload
                HttpContent content = new StringContent(jsonPayload);
                content.Headers.ContentType.MediaType = "application/json";

                // Send the POST request to the API endpoint
                HttpResponseMessage response = client.PostAsync(url, content).Result;

                // Check if the request is successful and redirect accordingly
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("List");
                }
                else
                {
                    Debug.WriteLine($"Failed to create veteran. Status code: {response.StatusCode}");
                    return RedirectToAction("Error");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred while creating veteran: {ex.Message}");
                return RedirectToAction("Error");
            }
        }



        // GET: Veteran/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "veterandata/findveteran/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            VeteranDto selectedVeteran = response.Content.ReadAsAsync<VeteranDto>().Result;
            Debug.WriteLine("Veteran received: ");

            return View(selectedVeteran);
        }

        // POST: Veteran/Update/5
        [HttpPost]
        public ActionResult Update(int id, Veteran veteran)
        {
            try
            {
                Debug.WriteLine("The new info is:");
                Debug.WriteLine(veteran.VeteranID);

                string url = "veterandata/UpdateVeteran/" + id;

                string jsonPayload = jss.Serialize(veteran);
                Debug.WriteLine(jsonPayload);

                HttpContent content = new StringContent(jsonPayload);
                content.Headers.ContentType.MediaType = "application/json";

                HttpResponseMessage response = client.PostAsync(url, content).Result;

                return RedirectToAction("Details/" + id);
            }
            catch
            {
                return View();
            }
        }
        // GET: Veteran/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "veterandata/findveteran/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            VeteranDto selectedVeteran = response.Content.ReadAsAsync<VeteranDto>().Result;
            return View(selectedVeteran);
        }

        // POST: Veteran/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                string url = "veterandata/deleteveteran/" + id;
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







    }
}