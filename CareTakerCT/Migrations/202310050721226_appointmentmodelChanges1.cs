namespace CareTakerCT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class appointmentmodelChanges1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Appointments", "Doctor_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Appointments", new[] { "Doctor_Id" });
            AlterColumn("dbo.Appointments", "Doctor_Id", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Appointments", "Doctor_Id");
            AddForeignKey("dbo.Appointments", "Doctor_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Appointments", "Doctor_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Appointments", new[] { "Doctor_Id" });
            AlterColumn("dbo.Appointments", "Doctor_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Appointments", "Doctor_Id");
            AddForeignKey("dbo.Appointments", "Doctor_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
