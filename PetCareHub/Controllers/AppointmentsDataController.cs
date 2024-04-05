using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using PassionProject_DentistAppointment.Models;
using PetCareHub.Models;

namespace PassionProject_DentistAppointment.Controllers
{
    public class AppointmentsDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/AppointmentsData/ListAppointment
        [HttpGet]
        public IEnumerable<AppointmentDto> ListAppointment()
        {
            List<Appointment> Appointments = db.Appointments.ToList();
            List<AppointmentDto> AppointmentDtos = new List<AppointmentDto>();

            Appointments.ForEach(a => AppointmentDtos.Add(new AppointmentDto()
            {
                AppointmentID = a.AppointmentID,
                AppointmentDate = a.AppointmentDate,

            }));

            return AppointmentDtos;
        }

        [HttpGet]
        [ResponseType(typeof(AppointmentDto))]
        public IHttpActionResult ListAppointmentForVeteran(int id)
        {

            List<Appointment> Appointments = db.Appointments.Where(
                k => k.Veterans.Any(
                    a => a.VeteranID == id)
                ).ToList();
            List<AppointmentDto> AppointmentDtos = new List<AppointmentDto>();

            Appointments.ForEach(k => AppointmentDtos.Add(new AppointmentDto()
            {
                AppointmentID = k.AppointmentID,
                AppointmentDate = k.AppointmentDate,

            }));

            return Ok(AppointmentDtos);
        }

        [HttpGet]
        [ResponseType(typeof(AppointmentDto))]
        public IHttpActionResult ListAppointmentNotForVeteran(int id)
        {
            List<Appointment> Appointments = db.Appointments.Where(
                k => !k.Veterans.Any(
                    a => a.VeteranID == id)
                ).ToList();
            List<AppointmentDto> AppointmentDtos = new List<AppointmentDto>();

            Appointments.ForEach(k => AppointmentDtos.Add(new AppointmentDto()
            {
                AppointmentID = k.AppointmentID,
                AppointmentDate = k.AppointmentDate,
            }));

            return Ok(AppointmentDtos);
        }
        [HttpPost]
        [Route("api/appointmentsdata/AssociateAppointmentWithVeteran/{veteranid}/{appointmentid}")]
        public IHttpActionResult AssociateAppointmentWithVeteran(int veteranid, int appointmentid)
        {

            Veteran selectedVeteran = db.Veterans.Include(a => a.Appointments).Where(a => a.VeteranID == veteranid).FirstOrDefault();
            Appointment selectedAppointment = db.Appointments.Find(appointmentid);

            if (selectedVeteran == null || selectedAppointment == null)
            {
                return NotFound();
            }

            Debug.WriteLine("Input veteran ID is: " + veteranid);
            Debug.WriteLine("Selected veteran name is: " + selectedVeteran.VeteranName);
            Debug.WriteLine("Input appointment ID is: " + appointmentid);
            Debug.WriteLine("Selected appointment date is: " + selectedAppointment.AppointmentDate);

            selectedVeteran.Appointments.Add(selectedAppointment);
            db.SaveChanges();

            return Ok();
        }
        [HttpPost]
        [Route("api/AppointmentsData/UnAssociateAppointmentWithVeteran/{veteranid}/{appointmentid}")]
        public IHttpActionResult UnAssociateAppointmentWithVeteran(int veteranid, int appointmentid)
        {

            Veteran selectedVeteran = db.Veterans.Include(a => a.Appointments).Where(a => a.VeteranID == veteranid).FirstOrDefault();
            Appointment selectedAppointment = db.Appointments.Find(appointmentid);

            if (selectedVeteran == null || selectedAppointment == null)
            {
                return NotFound();
            }

            //todo: verify that the veteran actually has the appointment

            selectedVeteran.Appointments.Remove(selectedAppointment);
            db.SaveChanges();

            return Ok();
        }


        // GET: api/AppointmentsData/FindAppointment/1
        [ResponseType(typeof(Appointment))]
        [HttpGet]
        public IHttpActionResult FindAppointment(int id)
        {
            Appointment appointment = db.Appointments.Find(id);
            AppointmentDto appointmentDto = new AppointmentDto()
            {
                AppointmentID = appointment.AppointmentID,
                AppointmentDate = appointment.AppointmentDate,
            };
            if (appointment == null)
            {
                return NotFound();
            }

            return Ok(appointmentDto);
        }

        // POST: api/AppointmentsData/UpdateAppointment/2
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateAppointment(int id, Appointment appointment)
        {

            if (!ModelState.IsValid)
            {
                Debug.WriteLine("Model State is invalid");
                return BadRequest(ModelState);
            }

            if (id != appointment.AppointmentID)
            {

                return BadRequest();
            }

            db.Entry(appointment).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentExists(id))
                {
                    Debug.WriteLine("Appointment not found");
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            Debug.WriteLine("None of the conditions triggered");
            return StatusCode(HttpStatusCode.NoContent);
        }
        // POST: api/AppointmentsData/AddAppointment
        [ResponseType(typeof(Appointment))]
        [HttpPost]
        public IHttpActionResult AddAppointment(Appointment appointment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Appointments.Add(appointment);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = appointment.AppointmentID }, appointment);
        }

        // POST: api/AppointmentData/DeleteAppointment/5
        [ResponseType(typeof(Appointment))]
        [HttpPost]
        public IHttpActionResult DeleteAppointment(int id)
        {
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return NotFound();
            }

            db.Appointments.Remove(appointment);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AppointmentExists(int id)
        {
            return db.Appointments.Count(e => e.AppointmentID == id) > 0;
        }
    }
}
