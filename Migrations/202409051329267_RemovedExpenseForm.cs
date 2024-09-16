namespace ExpenseApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedExpenseForm : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.VpExpenses", "ExpenseFormId", "dbo.VpExpenseForm");
            DropForeignKey("dbo.VpExpenseFormHistory", "ExpenseForm_Id", "dbo.VpExpenseForm");
            DropForeignKey("dbo.VpExpenseForm", "UserId", "dbo.VpUser");
            DropIndex("dbo.VpExpenseFormHistory", new[] { "ExpenseForm_Id" });
            DropIndex("dbo.VpExpenses", new[] { "ExpenseFormId" });
            DropIndex("dbo.VpExpenseForm", new[] { "UserId" });
            AddColumn("dbo.VpExpenses", "User_Id", c => c.Int());
            CreateIndex("dbo.VpExpenses", "User_Id");
            AddForeignKey("dbo.VpExpenses", "User_Id", "dbo.VpUser", "Id");
            DropColumn("dbo.VpExpenseFormHistory", "ExpenseForm_Id");
            DropColumn("dbo.VpExpenses", "ExpenseFormId");
            DropTable("dbo.VpExpenseForm");
        }
        
        public override void Down()
        {
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
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.VpExpenses", "ExpenseFormId", c => c.Int(nullable: false));
            AddColumn("dbo.VpExpenseFormHistory", "ExpenseForm_Id", c => c.Int());
            DropForeignKey("dbo.VpExpenses", "User_Id", "dbo.VpUser");
            DropIndex("dbo.VpExpenses", new[] { "User_Id" });
            DropColumn("dbo.VpExpenses", "User_Id");
            CreateIndex("dbo.VpExpenseForm", "UserId");
            CreateIndex("dbo.VpExpenses", "ExpenseFormId");
            CreateIndex("dbo.VpExpenseFormHistory", "ExpenseForm_Id");
            AddForeignKey("dbo.VpExpenseForm", "UserId", "dbo.VpUser", "Id", cascadeDelete: true);
            AddForeignKey("dbo.VpExpenseFormHistory", "ExpenseForm_Id", "dbo.VpExpenseForm", "Id");
            AddForeignKey("dbo.VpExpenses", "ExpenseFormId", "dbo.VpExpenseForm", "Id", cascadeDelete: true);
        }
    }
}
