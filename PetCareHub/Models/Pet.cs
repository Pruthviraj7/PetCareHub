using PetCareHub.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PassionProject_DentistAppointment.Models
{
    public class Pet
    {
        [Key]
        public int PetID { get; set; }
        public string PetName { get; set; }

        public string PetBreed { get; set; } // Changed from Breed to PetBreed

        public string PetGender { get; set; } // Changed from Gender to PetGender

        public int PetAge { get; set; } // Changed from Age to PetAge

        public double PetWeight { get; set; } // Changed from Weight to PetWeight

        // Navigation property for appointments
        public virtual ICollection<Appointment> Appointments { get; set; }

        public virtual ICollection<Veteran> Veterans { get; set; }
        public virtual ICollection<PetVeteranAppointment> PetVeteranAppointments { get; set; }
    }

    public class PetDto
    {
        public int PetID { get; set; }
        public string PetName { get; set; }

        public string PetBreed { get; set; }

        public string PetGender { get; set; }

        public int PetAge { get; set; }

        public double PetWeight { get; set; }
    }

}
