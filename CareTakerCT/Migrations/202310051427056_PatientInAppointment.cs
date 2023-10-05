namespace CareTakerCT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PatientInAppointment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Appointments", "PatientId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Appointments", "PatientId");
            AddForeignKey("dbo.Appointments", "PatientId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Appointments", "PatientId", "dbo.AspNetUsers");
            DropIndex("dbo.Appointments", new[] { "PatientId" });
            DropColumn("dbo.Appointments", "PatientId");
        }
    }
}
