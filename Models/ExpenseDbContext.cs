using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ExpenseApplication.Models
{
    public class ExpenseDbContext:DbContext
    {
        public DbSet<User> Users { get; set; }      
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<ExpenseFormHistory> ExpenseFormHistories { get; set; }

        public DbSet<ExpenseForm> ExpenseForms { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>().ToTable("VpUser");
            modelBuilder.Entity<Expense>().ToTable("VpExpenses");
            modelBuilder.Entity<ExpenseForm>().ToTable("VpExpenseForm");
            modelBuilder.Entity<ExpenseFormHistory>().ToTable("VpExpenseFormHistory");



            base.OnModelCreating(modelBuilder);
        }

        public System.Data.Entity.DbSet<ExpenseApplication.Models.ViewModels.ExpenseFormViewModel> ExpenseFormViewModels { get; set; }
    }
}