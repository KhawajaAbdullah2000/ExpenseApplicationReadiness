using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExpenseApplication.Models
{
    public class Expense
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime ExpenseDate { get; set; }

        [Range(0, 5000, ErrorMessage = "Expense amount cannot exceed 5000.")]
        public decimal Amount { get; set; }

        public ExpenseFormStatus Status { get; set; }  

      
    }
}