namespace ExpenseApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedExpenseFormAgainForManagers : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.VpExpenseFormHistory", "ExpenseId", "dbo.VpExpenses");
            DropForeignKey("dbo.VpExpenses", "User_Id", "dbo.VpUser");
            DropIndex("dbo.VpExpenseFormHistory", new[] { "ExpenseId" });
            DropIndex("dbo.VpExpenses", new[] { "User_Id" });
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
            
            AddColumn("dbo.VpExpenseFormHistory", "ExpenseFormId", c => c.Int(nullable: false));
            AddColumn("dbo.VpExpenses", "ExpenseForm_Id", c => c.Int());
            CreateIndex("dbo.VpExpenseFormHistory", "ExpenseFormId");
            CreateIndex("dbo.VpExpenses", "ExpenseForm_Id");
            AddForeignKey("dbo.VpExpenses", "ExpenseForm_Id", "dbo.VpExpenseForm", "Id");
            AddForeignKey("dbo.VpExpenseFormHistory", "ExpenseFormId", "dbo.VpExpenseForm", "Id", cascadeDelete: true);
            DropColumn("dbo.VpExpenseFormHistory", "ExpenseId");
            DropColumn("dbo.VpExpenses", "User_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.VpExpenses", "User_Id", c => c.Int());
            AddColumn("dbo.VpExpenseFormHistory", "ExpenseId", c => c.Int(nullable: false));
            DropForeignKey("dbo.VpExpenseForm", "UserId", "dbo.VpUser");
            DropForeignKey("dbo.VpExpenseFormHistory", "ExpenseFormId", "dbo.VpExpenseForm");
            DropForeignKey("dbo.VpExpenses", "ExpenseForm_Id", "dbo.VpExpenseForm");
            DropIndex("dbo.VpExpenses", new[] { "ExpenseForm_Id" });
            DropIndex("dbo.VpExpenseForm", new[] { "UserId" });
            DropIndex("dbo.VpExpenseFormHistory", new[] { "ExpenseFormId" });
            DropColumn("dbo.VpExpenses", "ExpenseForm_Id");
            DropColumn("dbo.VpExpenseFormHistory", "ExpenseFormId");
            DropTable("dbo.VpExpenseForm");
            CreateIndex("dbo.VpExpenses", "User_Id");
            CreateIndex("dbo.VpExpenseFormHistory", "ExpenseId");
            AddForeignKey("dbo.VpExpenses", "User_Id", "dbo.VpUser", "Id");
            AddForeignKey("dbo.VpExpenseFormHistory", "ExpenseId", "dbo.VpExpenses", "Id", cascadeDelete: true);
        }
    }
}
