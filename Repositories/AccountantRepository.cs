using ExpenseApplication.Interfaces;
using ExpenseApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace ExpenseApplication.Repositories
{
    public class AccountantRepository:IAccountant
    {
        private ExpenseDbContext context;

        public AccountantRepository(ExpenseDbContext dataContext)
        {
            this.context = dataContext;
        }

        public IQueryable<ExpenseForm> GetExpenseForms()
        {
            return context.ExpenseForms
               .Where(e => e.Status == ExpenseFormStatus.Approved)
               .AsQueryable();
        }

        public ExpenseForm GetExpenseFormDetails(int id)
        {
            return context.ExpenseForms
              .Include(f => f.Expenses)
              .FirstOrDefault(f => f.Id == id);
        }

        public ExpenseForm GetExpenseForm(int id)
        {
            return context.ExpenseForms.FirstOrDefault(f => f.Id == id);
        }

        public ExpenseFormHistory GetFormHistory(int id)
        {
            return context.ExpenseFormHistories.Where(f => f.ExpenseFormId == id)
                        .OrderByDescending(f => f.ChangeDate).Take(1).FirstOrDefault();
        }

        public void AddExpenseFormHistory(ExpenseFormHistory history)
        {
            context.ExpenseFormHistories.Add(history);
        }

        public User GetPortfolio(int id)
        {
            return context.Users.Where(e => e.Id == id).SingleOrDefault();
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}