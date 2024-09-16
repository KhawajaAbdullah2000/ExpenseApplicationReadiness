namespace ExpenseApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedEMployeeAdminAccountant : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.VpExpenseForm", "EmployeeId", "dbo.VpEmployee");
            DropForeignKey("dbo.VpAccountant", "Id", "dbo.VpUser");
            DropForeignKey("dbo.VpAdmin", "Id", "dbo.VpUser");
            DropForeignKey("dbo.VpEmployee", "Id", "dbo.VpUser");
            DropForeignKey("dbo.VpEmployee", "Manager_Id", "dbo.VpManager");
            DropForeignKey("dbo.VpManager", "Id", "dbo.VpUser");
            DropIndex("dbo.VpExpenseForm", new[] { "EmployeeId" });
            DropIndex("dbo.VpAccountant", new[] { "Id" });
            DropIndex("dbo.VpAdmin", new[] { "Id" });
            DropIndex("dbo.VpEmployee", new[] { "Id" });
            DropIndex("dbo.VpEmployee", new[] { "Manager_Id" });
            DropIndex("dbo.VpManager", new[] { "Id" });
            AddColumn("dbo.VpUser", "Email", c => c.String());
            AddColumn("dbo.VpUser", "ManagerId", c => c.Int());
            AddColumn("dbo.VpExpenseForm", "UserId", c => c.Int(nullable: false));
            AlterColumn("dbo.VpUser", "Role", c => c.Int(nullable: false));
            CreateIndex("dbo.VpExpenseForm", "UserId");
            CreateIndex("dbo.VpUser", "ManagerId");
            AddForeignKey("dbo.VpUser", "ManagerId", "dbo.VpUser", "Id");
            AddForeignKey("dbo.VpExpenseForm", "UserId", "dbo.VpUser", "Id", cascadeDelete: true);
            DropColumn("dbo.VpExpenseForm", "EmployeeId");
            DropTable("dbo.VpAccountant");
            DropTable("dbo.VpAdmin");
            DropTable("dbo.VpEmployee");
            DropTable("dbo.VpManager");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.VpManager",
                c => new
                    {
                        Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VpEmployee",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Manager_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VpAdmin",
                c => new
                    {
                        Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VpAccountant",
                c => new
                    {
                        Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.VpExpenseForm", "EmployeeId", c => c.Int(nullable: false));
            DropForeignKey("dbo.VpExpenseForm", "UserId", "dbo.VpUser");
            DropForeignKey("dbo.VpUser", "ManagerId", "dbo.VpUser");
            DropIndex("dbo.VpUser", new[] { "ManagerId" });
            DropIndex("dbo.VpExpenseForm", new[] { "UserId" });
            AlterColumn("dbo.VpUser", "Role", c => c.String());
            DropColumn("dbo.VpExpenseForm", "UserId");
            DropColumn("dbo.VpUser", "ManagerId");
            DropColumn("dbo.VpUser", "Email");
            CreateIndex("dbo.VpManager", "Id");
            CreateIndex("dbo.VpEmployee", "Manager_Id");
            CreateIndex("dbo.VpEmployee", "Id");
            CreateIndex("dbo.VpAdmin", "Id");
            CreateIndex("dbo.VpAccountant", "Id");
            CreateIndex("dbo.VpExpenseForm", "EmployeeId");
            AddForeignKey("dbo.VpManager", "Id", "dbo.VpUser", "Id");
            AddForeignKey("dbo.VpEmployee", "Manager_Id", "dbo.VpManager", "Id");
            AddForeignKey("dbo.VpEmployee", "Id", "dbo.VpUser", "Id");
            AddForeignKey("dbo.VpAdmin", "Id", "dbo.VpUser", "Id");
            AddForeignKey("dbo.VpAccountant", "Id", "dbo.VpUser", "Id");
            AddForeignKey("dbo.VpExpenseForm", "EmployeeId", "dbo.VpEmployee", "Id");
        }
    }
}
