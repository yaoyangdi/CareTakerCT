namespace CareTakerCT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ratingModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ratings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Value = c.Single(nullable: false),
                        Comments = c.String(),
                        DoctorId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.DoctorId)
                .Index(t => t.DoctorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ratings", "DoctorId", "dbo.AspNetUsers");
            DropIndex("dbo.Ratings", new[] { "DoctorId" });
            DropTable("dbo.Ratings");
        }
    }
}
