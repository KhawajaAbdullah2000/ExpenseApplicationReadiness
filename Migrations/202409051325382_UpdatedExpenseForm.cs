namespace ExpenseApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedExpenseForm : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.VpExpenseFormHistory", "ExpenseFormId", "dbo.VpExpenseForm");
            DropIndex("dbo.VpExpenseFormHistory", new[] { "ExpenseFormId" });
            RenameColumn(table: "dbo.VpExpenseFormHistory", name: "ExpenseFormId", newName: "ExpenseForm_Id");
            AddColumn("dbo.VpExpenseFormHistory", "ExpenseId", c => c.Int(nullable: false));
            AddColumn("dbo.VpExpenses", "Status", c => c.Int(nullable: false));
            AlterColumn("dbo.VpExpenseFormHistory", "ExpenseForm_Id", c => c.Int());
            CreateIndex("dbo.VpExpenseFormHistory", "ExpenseId");
            CreateIndex("dbo.VpExpenseFormHistory", "ExpenseForm_Id");
            AddForeignKey("dbo.VpExpenseFormHistory", "ExpenseId", "dbo.VpExpenses", "Id", cascadeDelete: true);
            AddForeignKey("dbo.VpExpenseFormHistory", "ExpenseForm_Id", "dbo.VpExpenseForm", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VpExpenseFormHistory", "ExpenseForm_Id", "dbo.VpExpenseForm");
            DropForeignKey("dbo.VpExpenseFormHistory", "ExpenseId", "dbo.VpExpenses");
            DropIndex("dbo.VpExpenseFormHistory", new[] { "ExpenseForm_Id" });
            DropIndex("dbo.VpExpenseFormHistory", new[] { "ExpenseId" });
            AlterColumn("dbo.VpExpenseFormHistory", "ExpenseForm_Id", c => c.Int(nullable: false));
            DropColumn("dbo.VpExpenses", "Status");
            DropColumn("dbo.VpExpenseFormHistory", "ExpenseId");
            RenameColumn(table: "dbo.VpExpenseFormHistory", name: "ExpenseForm_Id", newName: "ExpenseFormId");
            CreateIndex("dbo.VpExpenseFormHistory", "ExpenseFormId");
            AddForeignKey("dbo.VpExpenseFormHistory", "ExpenseFormId", "dbo.VpExpenseForm", "Id", cascadeDelete: true);
        }
    }
}
