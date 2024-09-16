using ExpenseApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseApplication.Interfaces
{
    public interface IAccountant
    {
        IQueryable<ExpenseForm> GetExpenseForms();

        ExpenseForm GetExpenseFormDetails(int id);

        ExpenseForm GetExpenseForm(int id);

        void SaveChanges();

        ExpenseFormHistory GetFormHistory(int id);

        User GetPortfolio(int id);

        void AddExpenseFormHistory(ExpenseFormHistory history);
    }
}
