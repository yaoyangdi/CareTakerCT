namespace CareTakerCT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class appointmentmodelChanges : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Appointments", "DoctorId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Appointments", "DoctorId", c => c.Int(nullable: false));
        }
    }
}
