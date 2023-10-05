namespace CareTakerCT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AppointmentModelUpdated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Appointments", "ClinicId", c => c.Int(nullable: false));
            AddColumn("dbo.Appointments", "DoctorId", c => c.Int(nullable: false));
            AddColumn("dbo.Appointments", "Doctor_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Appointments", "ClinicId");
            CreateIndex("dbo.Appointments", "Doctor_Id");
            AddForeignKey("dbo.Appointments", "ClinicId", "dbo.Clinics", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Appointments", "Doctor_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Appointments", "Doctor_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Appointments", "ClinicId", "dbo.Clinics");
            DropIndex("dbo.Appointments", new[] { "Doctor_Id" });
            DropIndex("dbo.Appointments", new[] { "ClinicId" });
            DropColumn("dbo.Appointments", "Doctor_Id");
            DropColumn("dbo.Appointments", "DoctorId");
            DropColumn("dbo.Appointments", "ClinicId");
        }
    }
}
