namespace ExpenseApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
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
                "dbo.VpExpenseForm",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        DateSubmitted = c.DateTime(nullable: false),
                        Currency = c.String(),
                        Status = c.Int(nullable: false),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.VpUser", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.VpExpenses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        ExpenseDate = c.DateTime(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExpenseForm_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.VpExpenseForm", t => t.ExpenseForm_Id)
                .Index(t => t.ExpenseForm_Id);
            
            CreateTable(
                "dbo.VpUser",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Password = c.String(),
                        Email = c.String(),
                        Role = c.Int(nullable: false),
                        ManagerId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.VpUser", t => t.ManagerId)
                .Index(t => t.ManagerId);
            
            CreateTable(
                "dbo.ExpenseFormViewModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Currency = c.String(nullable: false),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DateSubmitted = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ExpenseViewModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false, maxLength: 200),
                        ExpenseDate = c.DateTime(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExpenseFormViewModel_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ExpenseFormViewModels", t => t.ExpenseFormViewModel_Id)
                .Index(t => t.ExpenseFormViewModel_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ExpenseViewModels", "ExpenseFormViewModel_Id", "dbo.ExpenseFormViewModels");
            DropForeignKey("dbo.VpExpenseForm", "UserId", "dbo.VpUser");
            DropForeignKey("dbo.VpUser", "ManagerId", "dbo.VpUser");
            DropForeignKey("dbo.VpExpenseFormHistory", "ExpenseFormId", "dbo.VpExpenseForm");
            DropForeignKey("dbo.VpExpenses", "ExpenseForm_Id", "dbo.VpExpenseForm");
            DropIndex("dbo.ExpenseViewModels", new[] { "ExpenseFormViewModel_Id" });
            DropIndex("dbo.VpUser", new[] { "ManagerId" });
            DropIndex("dbo.VpExpenses", new[] { "ExpenseForm_Id" });
            DropIndex("dbo.VpExpenseForm", new[] { "UserId" });
            DropIndex("dbo.VpExpenseFormHistory", new[] { "ExpenseFormId" });
            DropTable("dbo.ExpenseViewModels");
            DropTable("dbo.ExpenseFormViewModels");
            DropTable("dbo.VpUser");
            DropTable("dbo.VpExpenses");
            DropTable("dbo.VpExpenseForm");
            DropTable("dbo.VpExpenseFormHistory");
        }
    }
}
