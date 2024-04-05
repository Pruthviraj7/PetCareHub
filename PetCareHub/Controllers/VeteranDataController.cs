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
using PassionProject_DentistAppointment.Models;
using PetCareHub.Models;

namespace PassionProject_DentistAppointment.Controllers
{
    public class VeteranDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/VeteranData/ListVeteran
        [HttpGet]
        public IEnumerable<VeteranDto> ListVeterans()
        {
            List<Veteran> veterans = db.Veterans.ToList();
            List<VeteranDto> veteranDtos = new List<VeteranDto>();

            veterans.ForEach(v => veteranDtos.Add(new VeteranDto()
            {
                VeteranID = v.VeteranID,
                VeteranName = v.VeteranName,
                VeteranSpecialization = v.VeteranSpecialization,
                VeteranPhone = v.VeteranPhone,
                VeteranEmail = v.VeteranEmail,
                VeteranAddress = v.VeteranAddress,
                VeteranRating = v.VeteranRating,
            }));

            return veteranDtos;
        }

        // GET: api/VeteranData/FindVeteran/1
        [ResponseType(typeof(VeteranDto))]
        [HttpGet]
        public IHttpActionResult FindVeteran(int id)
        {
            Veteran veteran = db.Veterans.Find(id);
            if (veteran == null)
            {
                return NotFound();
            }

            VeteranDto veteranDto = new VeteranDto()
            {
                VeteranID = veteran.VeteranID,
                VeteranName = veteran.VeteranName,
                VeteranSpecialization = veteran.VeteranSpecialization,
                VeteranPhone = veteran.VeteranPhone,
                VeteranEmail = veteran.VeteranEmail,
                VeteranAddress = veteran.VeteranAddress,
                VeteranRating = veteran.VeteranRating,
            };

            return Ok(veteranDto);
        }

        // POST: api/VeteranData/UpdateVeteran/2
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateVeteran(int id, Veteran veteran)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != veteran.VeteranID)
            {
                return BadRequest();
            }

            db.Entry(veteran).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VeteranExists(id))
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

        // POST: api/VeteranData/AddVeteran
        [ResponseType(typeof(Veteran))]
        [HttpPost]
        public IHttpActionResult AddVeteran(Veteran veteran)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Veterans.Add(veteran);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = veteran.VeteranID }, veteran);
        }

        // POST: api/VeteranData/DeleteVeteran/5
        [ResponseType(typeof(Veteran))]
        [HttpPost]
        public IHttpActionResult DeleteVeteran(int id)
        {
            Veteran veteran = db.Veterans.Find(id);
            if (veteran == null)
            {
                return NotFound();
            }

            db.Veterans.Remove(veteran);
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

        private bool VeteranExists(int id)
        {
            return db.Veterans.Count(e => e.VeteranID == id) > 0;
        }
    }
}
