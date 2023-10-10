namespace CareTakerCT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class imagesToFiles : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Images", newName: "Files");
            CreateTable(
                "dbo.SendEmails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ToEmail = c.String(nullable: false),
                        Subject = c.String(nullable: false),
                        Contents = c.String(nullable: false),
                        file_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Files", t => t.file_Id)
                .Index(t => t.file_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SendEmails", "file_Id", "dbo.Files");
            DropIndex("dbo.SendEmails", new[] { "file_Id" });
            DropTable("dbo.SendEmails");
            RenameTable(name: "dbo.Files", newName: "Images");
        }
    }
}
