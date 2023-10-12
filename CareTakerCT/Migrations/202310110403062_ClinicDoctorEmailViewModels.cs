namespace CareTakerCT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ClinicDoctorEmailViewModels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ClinicDoctorEmailViewModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        File_Id = c.Int(),
                        SendEmail_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Files", t => t.File_Id)
                .ForeignKey("dbo.SendEmails", t => t.SendEmail_Id)
                .Index(t => t.File_Id)
                .Index(t => t.SendEmail_Id);
            
            AddColumn("dbo.Clinics", "ClinicDoctorEmailViewModels_Id", c => c.Int());
            AddColumn("dbo.AspNetUsers", "ClinicDoctorEmailViewModels_Id", c => c.Int());
            CreateIndex("dbo.Clinics", "ClinicDoctorEmailViewModels_Id");
            CreateIndex("dbo.AspNetUsers", "ClinicDoctorEmailViewModels_Id");
            AddForeignKey("dbo.Clinics", "ClinicDoctorEmailViewModels_Id", "dbo.ClinicDoctorEmailViewModels", "Id");
            AddForeignKey("dbo.AspNetUsers", "ClinicDoctorEmailViewModels_Id", "dbo.ClinicDoctorEmailViewModels", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ClinicDoctorEmailViewModels", "SendEmail_Id", "dbo.SendEmails");
            DropForeignKey("dbo.ClinicDoctorEmailViewModels", "File_Id", "dbo.Files");
            DropForeignKey("dbo.AspNetUsers", "ClinicDoctorEmailViewModels_Id", "dbo.ClinicDoctorEmailViewModels");
            DropForeignKey("dbo.Clinics", "ClinicDoctorEmailViewModels_Id", "dbo.ClinicDoctorEmailViewModels");
            DropIndex("dbo.ClinicDoctorEmailViewModels", new[] { "SendEmail_Id" });
            DropIndex("dbo.ClinicDoctorEmailViewModels", new[] { "File_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "ClinicDoctorEmailViewModels_Id" });
            DropIndex("dbo.Clinics", new[] { "ClinicDoctorEmailViewModels_Id" });
            DropColumn("dbo.AspNetUsers", "ClinicDoctorEmailViewModels_Id");
            DropColumn("dbo.Clinics", "ClinicDoctorEmailViewModels_Id");
            DropTable("dbo.ClinicDoctorEmailViewModels");
        }
    }
}
