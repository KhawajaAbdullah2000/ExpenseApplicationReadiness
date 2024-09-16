using ExpenseApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseApplication.Interfaces
{
    public interface IManager
    {
        User GetEmployeesUnderManager(int id);
        IQueryable<ExpenseForm> GetExpenseForms(User manager);

        ExpenseForm GetExpenseFormDetails(int id);

        ExpenseForm GetExpenseForm(int id);

        void SaveChanges();

        ExpenseFormHistory GetFormHistory(int id);

        void AddExpenseFormHistory(ExpenseFormHistory history);

        User GetEmployees(int managerId);
    }
}
