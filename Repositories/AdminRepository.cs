using ExpenseApplication.Interfaces;
using ExpenseApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using ExpenseApplication.Models.ChartModels;

namespace ExpenseApplication.Repositories
{
    public class AdminRepository:IAdmin

    {
        private ExpenseDbContext context;

        public AdminRepository(ExpenseDbContext dataContext)
        {
            this.context = dataContext;
        }

        public IQueryable<ExpenseForm> GetExpenseForms()
        {
            return context.ExpenseForms.OrderByDescending(e => e.DateSubmitted).AsQueryable();
        }

        public ExpenseForm GetExpenseFormDetails(int id)
        {
            return context.ExpenseForms
              .Include(f => f.Expenses)
              .FirstOrDefault(f => f.Id == id);
        }

        public IEnumerable<ExpenseFormHistory> GetTransactions(int id)
        {
           return context.ExpenseFormHistories.Where(f => f.ExpenseFormId == id).ToList();
        }

        public IEnumerable<StatusCount> GetExpenseFormStatusCounts()
        {
            return context.ExpenseForms
                          .GroupBy(e => e.Status)
                          .Select(g => new StatusCount
                          {
                              Status = g.Key.ToString(),
                              Count = g.Count()
                          })
                          .ToList();
        }

        public IEnumerable<EmployeeFormCount> GetEmployeeFormCounts()
        {
            return context.Users
                          .Where(u => u.Role == UserRole.Employee)
                          .Select(u => new EmployeeFormCount
                          {
                              EmployeeName = u.Username,
                              FormCount = u.ExpenseForms.Count()
                          })
                          .ToList();
        }

        public IEnumerable<ManagerEmployeeCount> GetManagerEmployeeCounts()
        {
            return context.Users
                          .Where(u => u.Role == UserRole.Manager)
                          .Select(m => new ManagerEmployeeCount
                          {
                              ManagerName = m.Username,
                              EmployeeCount = m.Employees.Count()
                          })
                          .ToList();
        }

        public IEnumerable<DatewiseExpense> GetDatewiseExpenseTotals()
        {
            return context.ExpenseForms.Where(e=>e.Status==ExpenseFormStatus.Paid)
             .GroupBy(e => DbFunctions.TruncateTime(e.DateSubmitted)) // Extract the date part only
             .Select(g => new DatewiseExpense
             {
                 Date = g.Key,
                 TotalAmount = g.Sum(e => e.TotalAmount)
             })
             .OrderBy(e => e.Date) // Ensure the data is sorted by date
             .ToList();
        }

        public IEnumerable<CurrencyExpenseSummary> GetCurrencyExpenseSummaries()
        {
            return context.ExpenseForms.Where(e => e.Status == ExpenseFormStatus.Paid)
                          .GroupBy(e => e.Currency)
                          .Select(g => new CurrencyExpenseSummary
                          {
                              Currency = g.Key,
                              TotalAmount = g.Sum(e => e.TotalAmount)
                          })
                          .ToList();
        }


    }
}