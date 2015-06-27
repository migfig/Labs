namespace WebApi.Example.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        GroupNumber = c.Int(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        EmailAddress = c.String(),
                        Created = c.DateTime(nullable: false),
                        Expires = c.DateTime(nullable: false),
                        IsLocked = c.Boolean(nullable: false),
                        Group_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Groups", t => t.Group_Id)
                .Index(t => t.Group_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "Group_Id", "dbo.Groups");
            DropIndex("dbo.Users", new[] { "Group_Id" });
            DropTable("dbo.Users");
            DropTable("dbo.Groups");
        }
    }
}
