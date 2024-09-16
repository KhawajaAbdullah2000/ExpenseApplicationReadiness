namespace ExpenseApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VpUser",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Password = c.String(),
                        Role = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VpExpenseForm",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmployeeId = c.Int(nullable: false),
                        DateSubmitted = c.DateTime(nullable: false),
                        Currency = c.String(),
                        Status = c.Int(nullable: false),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.VpEmployee", t => t.EmployeeId)
                .Index(t => t.EmployeeId);
            
            CreateTable(
                "dbo.VpExpenses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ExpenseFormId = c.Int(nullable: false),
                        Description = c.String(),
                        ExpenseDate = c.DateTime(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.VpExpenseForm", t => t.ExpenseFormId, cascadeDelete: true)
                .Index(t => t.ExpenseFormId);
            
            CreateTable(
                "dbo.VpExpenseFormHistory",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ExpenseFormId = c.Int(nullable: false),
                        ChangeDate = c.DateTime(nullable: false),
                        OldStatus = c.Int(nullable: false),
                        NewStatus = c.Int(nullable: false),
                        ActionBy = c.String(),
                        Remarks = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.VpExpenseForm", t => t.ExpenseFormId, cascadeDelete: true)
                .Index(t => t.ExpenseFormId);
            
            CreateTable(
                "dbo.VpAccountant",
                c => new
                    {
                        Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.VpUser", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.VpAdmin",
                c => new
                    {
                        Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.VpUser", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.VpEmployee",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Manager_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.VpUser", t => t.Id)
                .ForeignKey("dbo.VpManager", t => t.Manager_Id)
                .Index(t => t.Id)
                .Index(t => t.Manager_Id);
            
            CreateTable(
                "dbo.VpManager",
                c => new
                    {
                        Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.VpUser", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VpManager", "Id", "dbo.VpUser");
            DropForeignKey("dbo.VpEmployee", "Manager_Id", "dbo.VpManager");
            DropForeignKey("dbo.VpEmployee", "Id", "dbo.VpUser");
            DropForeignKey("dbo.VpAdmin", "Id", "dbo.VpUser");
            DropForeignKey("dbo.VpAccountant", "Id", "dbo.VpUser");
            DropForeignKey("dbo.VpExpenseFormHistory", "ExpenseFormId", "dbo.VpExpenseForm");
            DropForeignKey("dbo.VpExpenses", "ExpenseFormId", "dbo.VpExpenseForm");
            DropForeignKey("dbo.VpExpenseForm", "EmployeeId", "dbo.VpEmployee");
            DropIndex("dbo.VpManager", new[] { "Id" });
            DropIndex("dbo.VpEmployee", new[] { "Manager_Id" });
            DropIndex("dbo.VpEmployee", new[] { "Id" });
            DropIndex("dbo.VpAdmin", new[] { "Id" });
            DropIndex("dbo.VpAccountant", new[] { "Id" });
            DropIndex("dbo.VpExpenseFormHistory", new[] { "ExpenseFormId" });
            DropIndex("dbo.VpExpenses", new[] { "ExpenseFormId" });
            DropIndex("dbo.VpExpenseForm", new[] { "EmployeeId" });
            DropTable("dbo.VpManager");
            DropTable("dbo.VpEmployee");
            DropTable("dbo.VpAdmin");
            DropTable("dbo.VpAccountant");
            DropTable("dbo.VpExpenseFormHistory");
            DropTable("dbo.VpExpenses");
            DropTable("dbo.VpExpenseForm");
            DropTable("dbo.VpUser");
        }
    }
}
