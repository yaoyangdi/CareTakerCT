namespace CareTakerCT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class appointmentDoctorIdType : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Appointments", name: "Doctor_Id", newName: "DoctorId");
            RenameIndex(table: "dbo.Appointments", name: "IX_Doctor_Id", newName: "IX_DoctorId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Appointments", name: "IX_DoctorId", newName: "IX_Doctor_Id");
            RenameColumn(table: "dbo.Appointments", name: "DoctorId", newName: "Doctor_Id");
        }
    }
}
