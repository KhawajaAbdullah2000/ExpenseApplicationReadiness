using ExpenseApplication.Models;
using ExpenseApplication.Models.ChartModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseApplication.Interfaces
{
    public interface IEmployee
    {
        void AddExpense(ExpenseForm form);

        void AddExpenseFormHistory(ExpenseFormHistory history);
        ExpenseForm GetExpenseFormById(int id);
        ExpenseForm GetExpensesByFormId(int formId);
        void RemoveExpenses(IEnumerable<Expense> expenses);

        IQueryable<ExpenseForm> GetExpenseFormsByUserId(int userId);

     
        void SaveChanges();
    }
}
