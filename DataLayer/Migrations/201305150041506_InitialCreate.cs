namespace DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ExpressionOfInterest",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50, fixedLength: true),
                        Phone = c.String(maxLength: 50, fixedLength: true),
                        Email = c.String(nullable: false, maxLength: 50, fixedLength: true),
                        Message = c.String(nullable: false, maxLength: 1500, fixedLength: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AddressType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(nullable: false, maxLength: 20, fixedLength: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Address",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AddressLineOne = c.String(nullable: false, maxLength: 30, fixedLength: true),
                        AddressLineTwo = c.String(maxLength: 30, fixedLength: true),
                        Suburb = c.String(nullable: false, maxLength: 30, fixedLength: true),
                        State = c.String(nullable: false, maxLength: 30, fixedLength: true),
                        City = c.String(nullable: false, maxLength: 30, fixedLength: true),
                        Country = c.String(nullable: false, maxLength: 30, fixedLength: true),
                        Type = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AddressType", t => t.Type, cascadeDelete: true)
                .ForeignKey("dbo.UserProfile", t => t.UserId, cascadeDelete: true)
                .Index(t => t.Type)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserProfile",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        Email = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        UserName = c.String(),
                        LastName = c.String(),
                        FirstName = c.String(),
                        DateOfBirth = c.DateTime(),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.Investments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateOfInvestment = c.DateTime(),
                        InvestedAmount = c.Double(),
                        Maturity = c.DateTime(),
                        FinalAmount = c.Double(),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserProfile", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Phone",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PhoneNumber = c.String(nullable: false, maxLength: 15, fixedLength: true),
                        Type = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PhoneType", t => t.Type, cascadeDelete: true)
                .ForeignKey("dbo.UserProfile", t => t.UserId, cascadeDelete: true)
                .Index(t => t.Type)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.PhoneType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(nullable: false, maxLength: 20, fixedLength: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Product",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50, fixedLength: true),
                        Description = c.String(nullable: false, maxLength: 50, fixedLength: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UpcomingProjects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50, fixedLength: true),
                        Description = c.String(maxLength: 10, fixedLength: true),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Phone", new[] { "UserId" });
            DropIndex("dbo.Phone", new[] { "Type" });
            DropIndex("dbo.Investments", new[] { "UserId" });
            DropIndex("dbo.Address", new[] { "UserId" });
            DropIndex("dbo.Address", new[] { "Type" });
            DropForeignKey("dbo.Phone", "UserId", "dbo.UserProfile");
            DropForeignKey("dbo.Phone", "Type", "dbo.PhoneType");
            DropForeignKey("dbo.Investments", "UserId", "dbo.UserProfile");
            DropForeignKey("dbo.Address", "UserId", "dbo.UserProfile");
            DropForeignKey("dbo.Address", "Type", "dbo.AddressType");
            DropTable("dbo.UpcomingProjects");
            DropTable("dbo.Product");
            DropTable("dbo.PhoneType");
            DropTable("dbo.Phone");
            DropTable("dbo.Investments");
            DropTable("dbo.UserProfile");
            DropTable("dbo.Address");
            DropTable("dbo.AddressType");
            DropTable("dbo.ExpressionOfInterest");
        }
    }
}
