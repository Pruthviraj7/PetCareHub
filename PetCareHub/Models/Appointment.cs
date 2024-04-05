using PetCareHub.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PassionProject_DentistAppointment.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentID { get; set; }
        public DateTime AppointmentDate { get; set; }

        // Navigation property for veterans
        public virtual ICollection<Veteran> Veterans { get; set; }

        // Navigation property for pets
        public virtual ICollection<Pet> Pets { get; set; }

        // Navigation property for connecting appointments with pets and veterans
        public virtual ICollection<PetVeteranAppointment> PetVeteranAppointments { get; set; }
    }

    public class AppointmentDto
    {
        public int AppointmentID { get; set; }
        public DateTime AppointmentDate { get; set; }
    }
}
