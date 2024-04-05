namespace PetCareHub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class petdbo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Appointments",
                c => new
                    {
                        AppointmentID = c.Int(nullable: false, identity: true),
                        AppointmentDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.AppointmentID);
            
            CreateTable(
                "dbo.PetVeteranAppointments",
                c => new
                    {
                        PetVeteranAppointmentID = c.Int(nullable: false, identity: true),
                        PetID = c.Int(nullable: false),
                        VeteranID = c.Int(nullable: false),
                        AppointmentID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PetVeteranAppointmentID)
                .ForeignKey("dbo.Appointments", t => t.AppointmentID, cascadeDelete: true)
                .ForeignKey("dbo.Pets", t => t.PetID, cascadeDelete: true)
                .ForeignKey("dbo.Veterans", t => t.VeteranID, cascadeDelete: true)
                .Index(t => t.PetID)
                .Index(t => t.VeteranID)
                .Index(t => t.AppointmentID);
            
            CreateTable(
                "dbo.Veterans",
                c => new
                    {
                        VeteranID = c.Int(nullable: false, identity: true),
                        VeteranName = c.String(),
                        VeteranSpecialization = c.String(),
                        VeteranPhone = c.String(),
                        VeteranEmail = c.String(),
                        VeteranAddress = c.String(),
                        VeteranRating = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.VeteranID);
            
            CreateTable(
                "dbo.AppointmentPets",
                c => new
                    {
                        Appointment_AppointmentID = c.Int(nullable: false),
                        Pet_PetID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Appointment_AppointmentID, t.Pet_PetID })
                .ForeignKey("dbo.Appointments", t => t.Appointment_AppointmentID, cascadeDelete: true)
                .ForeignKey("dbo.Pets", t => t.Pet_PetID, cascadeDelete: true)
                .Index(t => t.Appointment_AppointmentID)
                .Index(t => t.Pet_PetID);
            
            CreateTable(
                "dbo.VeteranAppointments",
                c => new
                    {
                        Veteran_VeteranID = c.Int(nullable: false),
                        Appointment_AppointmentID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Veteran_VeteranID, t.Appointment_AppointmentID })
                .ForeignKey("dbo.Veterans", t => t.Veteran_VeteranID, cascadeDelete: true)
                .ForeignKey("dbo.Appointments", t => t.Appointment_AppointmentID, cascadeDelete: true)
                .Index(t => t.Veteran_VeteranID)
                .Index(t => t.Appointment_AppointmentID);
            
            CreateTable(
                "dbo.VeteranPets",
                c => new
                    {
                        Veteran_VeteranID = c.Int(nullable: false),
                        Pet_PetID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Veteran_VeteranID, t.Pet_PetID })
                .ForeignKey("dbo.Veterans", t => t.Veteran_VeteranID, cascadeDelete: true)
                .ForeignKey("dbo.Pets", t => t.Pet_PetID, cascadeDelete: true)
                .Index(t => t.Veteran_VeteranID)
                .Index(t => t.Pet_PetID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PetVeteranAppointments", "VeteranID", "dbo.Veterans");
            DropForeignKey("dbo.VeteranPets", "Pet_PetID", "dbo.Pets");
            DropForeignKey("dbo.VeteranPets", "Veteran_VeteranID", "dbo.Veterans");
            DropForeignKey("dbo.VeteranAppointments", "Appointment_AppointmentID", "dbo.Appointments");
            DropForeignKey("dbo.VeteranAppointments", "Veteran_VeteranID", "dbo.Veterans");
            DropForeignKey("dbo.PetVeteranAppointments", "PetID", "dbo.Pets");
            DropForeignKey("dbo.PetVeteranAppointments", "AppointmentID", "dbo.Appointments");
            DropForeignKey("dbo.AppointmentPets", "Pet_PetID", "dbo.Pets");
            DropForeignKey("dbo.AppointmentPets", "Appointment_AppointmentID", "dbo.Appointments");
            DropIndex("dbo.VeteranPets", new[] { "Pet_PetID" });
            DropIndex("dbo.VeteranPets", new[] { "Veteran_VeteranID" });
            DropIndex("dbo.VeteranAppointments", new[] { "Appointment_AppointmentID" });
            DropIndex("dbo.VeteranAppointments", new[] { "Veteran_VeteranID" });
            DropIndex("dbo.AppointmentPets", new[] { "Pet_PetID" });
            DropIndex("dbo.AppointmentPets", new[] { "Appointment_AppointmentID" });
            DropIndex("dbo.PetVeteranAppointments", new[] { "AppointmentID" });
            DropIndex("dbo.PetVeteranAppointments", new[] { "VeteranID" });
            DropIndex("dbo.PetVeteranAppointments", new[] { "PetID" });
            DropTable("dbo.VeteranPets");
            DropTable("dbo.VeteranAppointments");
            DropTable("dbo.AppointmentPets");
            DropTable("dbo.Veterans");
            DropTable("dbo.PetVeteranAppointments");
            DropTable("dbo.Appointments");
        }
    }
}
