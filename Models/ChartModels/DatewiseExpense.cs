using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExpenseApplication.Models.ChartModels
{
    public class DatewiseExpense
    {
        public DateTime? Date { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
