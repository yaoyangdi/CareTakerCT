namespace CareTakerCT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class doctorRating : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Ratings", newName: "DoctorRatings");
            AddColumn("dbo.DoctorRatings", "Comment", c => c.String());
            DropColumn("dbo.DoctorRatings", "Comments");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DoctorRatings", "Comments", c => c.String());
            DropColumn("dbo.DoctorRatings", "Comment");
            RenameTable(name: "dbo.DoctorRatings", newName: "Ratings");
        }
    }
}
