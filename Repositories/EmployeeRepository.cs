using ExpenseApplication.Interfaces;
using ExpenseApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Helpers;

namespace ExpenseApplication.Repositories
{
    public class EmployeeRepository:IEmployee
    {
        private ExpenseDbContext context;

        public EmployeeRepository(ExpenseDbContext dataContext)
        {
            this.context = dataContext;
        }

        public User FetchUser(LoginViewModel user)
        {
            var userRecord = context.Users
                              .Where(u => u.Email == user.Email)
                              .FirstOrDefault();
            if (userRecord != null && Crypto.VerifyHashedPassword(userRecord.Password, user.Password))
            {
                return userRecord;
            }

            return null;
        }
     

        public void AddExpense(ExpenseForm form)
        {
            context.ExpenseForms.Add(form);
        }

        public void AddExpenseFormHistory(ExpenseFormHistory history)
        {
            context.ExpenseFormHistories.Add(history);
        }

        public ExpenseForm GetExpenseFormById(int id)
        {
            return context.ExpenseForms.Include(f => f.Expenses)
                                        .FirstOrDefault(f => f.Id == id);
        }

        public ExpenseForm GetExpensesByFormId(int formId)
        {
            return context.ExpenseForms.Include(f => f.Expenses).FirstOrDefault(f => f.Id == formId);
        }

        public void RemoveExpenses(IEnumerable<Expense> expenses)
        {
            foreach (var expense in expenses)
            {
                context.Expenses.Remove(expense);
            }
        }

        public IQueryable<ExpenseForm> GetExpenseFormsByUserId(int userId)
        {
            return context.ExpenseForms
                           .Where(e => e.UserId == userId)
                           .OrderByDescending(e=>e.Id);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

    }
}