using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PassionProject_DentistAppointment.Models
{
    public class PetVeteranAppointment
    {
        [Key]
        public int PetVeteranAppointmentID { get; set; }

        // Foreign keys
        public int PetID { get; set; }
        public int VeteranID { get; set; }
        public int AppointmentID { get; set; }

        // Navigation properties
        public virtual Pet Pet { get; set; }
        public virtual Veteran Veteran { get; set; }
        public virtual Appointment Appointment { get; set; }
    }
}
