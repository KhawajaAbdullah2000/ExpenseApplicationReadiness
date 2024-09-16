using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExpenseApplication.Models
{
    public class ExpenseForm
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public DateTime DateSubmitted { get; set; }
        public string Currency { get; set; }
        public ExpenseFormStatus Status { get; set; } // PendingApproval, Approved, Paid, ChangeRequested, Rejected

        public virtual ICollection<Expense> Expenses { get; set; } 
        public decimal TotalAmount { get; set; }

        public virtual ICollection<ExpenseFormHistory> History { get; set; }
    }
}