namespace CareTakerCT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class filesLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Files", "Path", c => c.String(nullable: false));
            AlterColumn("dbo.Files", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Files", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Files", "Path", c => c.String(nullable: false, maxLength: 50));
        }
    }
}
