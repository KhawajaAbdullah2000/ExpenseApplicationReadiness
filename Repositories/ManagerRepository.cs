using ExpenseApplication.Interfaces;
using ExpenseApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Collections;

namespace ExpenseApplication.Repositories
{
    public class ManagerRepository:IManager
    {
        private ExpenseDbContext context;

        public ManagerRepository(ExpenseDbContext dataContext)
        {
            this.context = dataContext;
        }

        public User GetEmployeesUnderManager(int id)
        {
            var users =context.Users
                    .Include(u => u.Employees.Select(e => e.ExpenseForms))
                    .SingleOrDefault(u => u.Id == id);
            return users;
        }

        public IQueryable<ExpenseForm> GetExpenseForms(User manager)
        {
            return manager.Employees
               .SelectMany(e => e.ExpenseForms)
               .Where(f => f.Status != ExpenseFormStatus.Paid && f.Status != ExpenseFormStatus.Approved)
               .OrderByDescending(f=>f.Id)
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

        public User GetEmployees(int managerId)
        {
            return context.Users
           .Include(u => u.Employees) // Include employees
           .SingleOrDefault(u => u.Id == managerId);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}