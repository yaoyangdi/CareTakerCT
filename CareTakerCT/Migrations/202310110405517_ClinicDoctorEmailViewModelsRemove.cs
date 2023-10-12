namespace CareTakerCT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ClinicDoctorEmailViewModelsRemove : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Clinics", "ClinicDoctorEmailViewModels_Id", "dbo.ClinicDoctorEmailViewModels");
            DropForeignKey("dbo.AspNetUsers", "ClinicDoctorEmailViewModels_Id", "dbo.ClinicDoctorEmailViewModels");
            DropForeignKey("dbo.ClinicDoctorEmailViewModels", "File_Id", "dbo.Files");
            DropForeignKey("dbo.ClinicDoctorEmailViewModels", "SendEmail_Id", "dbo.SendEmails");
            DropIndex("dbo.Clinics", new[] { "ClinicDoctorEmailViewModels_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "ClinicDoctorEmailViewModels_Id" });
            DropIndex("dbo.ClinicDoctorEmailViewModels", new[] { "File_Id" });
            DropIndex("dbo.ClinicDoctorEmailViewModels", new[] { "SendEmail_Id" });
            DropColumn("dbo.Clinics", "ClinicDoctorEmailViewModels_Id");
            DropColumn("dbo.AspNetUsers", "ClinicDoctorEmailViewModels_Id");
            DropTable("dbo.ClinicDoctorEmailViewModels");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ClinicDoctorEmailViewModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        File_Id = c.Int(),
                        SendEmail_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.AspNetUsers", "ClinicDoctorEmailViewModels_Id", c => c.Int());
            AddColumn("dbo.Clinics", "ClinicDoctorEmailViewModels_Id", c => c.Int());
            CreateIndex("dbo.ClinicDoctorEmailViewModels", "SendEmail_Id");
            CreateIndex("dbo.ClinicDoctorEmailViewModels", "File_Id");
            CreateIndex("dbo.AspNetUsers", "ClinicDoctorEmailViewModels_Id");
            CreateIndex("dbo.Clinics", "ClinicDoctorEmailViewModels_Id");
            AddForeignKey("dbo.ClinicDoctorEmailViewModels", "SendEmail_Id", "dbo.SendEmails", "Id");
            AddForeignKey("dbo.ClinicDoctorEmailViewModels", "File_Id", "dbo.Files", "Id");
            AddForeignKey("dbo.AspNetUsers", "ClinicDoctorEmailViewModels_Id", "dbo.ClinicDoctorEmailViewModels", "Id");
            AddForeignKey("dbo.Clinics", "ClinicDoctorEmailViewModels_Id", "dbo.ClinicDoctorEmailViewModels", "Id");
        }
    }
}
