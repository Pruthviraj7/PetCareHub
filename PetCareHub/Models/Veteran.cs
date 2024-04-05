using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PetCareHub.Models;

namespace PassionProject_DentistAppointment.Models
{
    public class Veteran
    {

        [Key]
        public int VeteranID { get; set; }
        public string VeteranName { get; set; }

        public string VeteranSpecialization { get; set; }

        public string VeteranPhone { get; set; }

        public string VeteranEmail { get; set; }

        public string VeteranAddress { get; set; }

        public int VeteranRating { get; set; }
       

        // Navigation property for appointments
        public virtual ICollection<Appointment> Appointments { get; set; }

        public virtual ICollection<Pet> Pets { get; set; }
        public virtual ICollection<PetVeteranAppointment> PetVeteranAppointments { get; set; }
    }

    public class VeteranDto
    {
        public int VeteranID { get; set; }
        public string VeteranName { get; set; }

        public string VeteranSpecialization { get; set; }

        public string VeteranPhone { get; set; }

        public string VeteranEmail { get; set; }

        public string VeteranAddress { get; set; }

        public int VeteranRating { get; set; }
      
      
    }
}
