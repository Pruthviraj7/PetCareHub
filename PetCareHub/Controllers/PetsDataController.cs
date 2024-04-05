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
    public class PetsDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/PetsData/ListPets
        [HttpGet]
        public IEnumerable<PetDto> ListPets()
        {
            List<Pet> Pets = db.Pets.ToList();
            List<PetDto> PetDtoList = new List<PetDto>();

            Pets.ForEach(pet => PetDtoList.Add(new PetDto()
            {
                PetID = pet.PetID,
                PetName = pet.PetName,
                PetBreed = pet.PetBreed,
                PetGender = pet.PetGender,
                PetAge = pet.PetAge,
                PetWeight = pet.PetWeight
            }));

            return PetDtoList;
        }


        // GET: api/PetsData/FindPet/1
        [ResponseType(typeof(Pet))]
        [HttpGet]
        public IHttpActionResult FindPet(int id)
        {
            Pet pet = db.Pets.Find(id);
            PetDto petDto = new PetDto()
            {
                PetID = pet.PetID,
                PetName = pet.PetName,
                PetBreed = pet.PetBreed,
                PetGender = pet.PetGender,
                PetAge = pet.PetAge,
                PetWeight = pet.PetWeight
            };
            if (pet == null)
            {
                return NotFound();
            }

            return Ok(petDto);
        }

        // POST: api/PetsData/UpdatePet/2
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdatePet(int id, Pet pet)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pet.PetID)
            {
                return BadRequest();
            }

            db.Entry(pet).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PetExists(id))
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

        // POST: api/PetsData/AddPet
        [ResponseType(typeof(Pet))]
        [HttpPost]
        public IHttpActionResult AddPet(Pet pet)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Pets.Add(pet);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = pet.PetID }, pet);
        }

        // POST: api/PetsData/DeletePet/5
        [ResponseType(typeof(Pet))]
        [HttpPost]
        public IHttpActionResult DeletePet(int id)
        {
            Pet pet = db.Pets.Find(id);
            if (pet == null)
            {
                return NotFound();
            }

            db.Pets.Remove(pet);
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

        private bool PetExists(int id)
        {
            return db.Pets.Count(e => e.PetID == id) > 0;
        }
    }
}
