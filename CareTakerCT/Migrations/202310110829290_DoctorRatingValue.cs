namespace CareTakerCT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DoctorRatingValue : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.DoctorRatings", "Value", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DoctorRatings", "Value", c => c.Single(nullable: false));
        }
    }
}
