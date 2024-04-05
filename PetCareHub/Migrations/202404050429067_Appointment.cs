namespace PetCareHub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Appointment : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.AppointmentPets", newName: "PetAppointments");
            DropPrimaryKey("dbo.PetAppointments");
            AddPrimaryKey("dbo.PetAppointments", new[] { "Pet_PetID", "Appointment_AppointmentID" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.PetAppointments");
            AddPrimaryKey("dbo.PetAppointments", new[] { "Appointment_AppointmentID", "Pet_PetID" });
            RenameTable(name: "dbo.PetAppointments", newName: "AppointmentPets");
        }
    }
}
