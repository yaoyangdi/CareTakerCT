namespace CareTakerCT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class clinics : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clinics", "DoctorId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Clinics", "DoctorId");
        }
    }
}
