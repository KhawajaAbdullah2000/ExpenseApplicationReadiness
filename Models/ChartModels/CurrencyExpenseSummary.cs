using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExpenseApplication.Models.ChartModels
{
    public class CurrencyExpenseSummary
    {
        public string Currency { get; set; }
        public decimal TotalAmount { get; set; }
    }
}