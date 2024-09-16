using ExpenseApplication.Models;
using ExpenseApplication.Models.ChartModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseApplication.Interfaces
{
    public interface IAdmin
    {
        IQueryable<ExpenseForm> GetExpenseForms();

        ExpenseForm GetExpenseFormDetails(int id);

        IEnumerable<ExpenseFormHistory> GetTransactions(int id);

        IEnumerable<StatusCount> GetExpenseFormStatusCounts();

        IEnumerable<EmployeeFormCount> GetEmployeeFormCounts();

        IEnumerable<ManagerEmployeeCount> GetManagerEmployeeCounts();

        IEnumerable<DatewiseExpense> GetDatewiseExpenseTotals();

        IEnumerable<CurrencyExpenseSummary> GetCurrencyExpenseSummaries();


    }
}
