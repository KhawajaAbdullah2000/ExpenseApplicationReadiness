using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExpenseApplication.Models
{
    public class ExpenseFormHistory
    {

        public int Id { get; set; }
        public int ExpenseFormId { get; set; }
        public virtual ExpenseForm ExpenseForm { get; set; }
        public DateTime ChangeDate { get; set; }
        public ExpenseFormStatus OldStatus { get; set; }
        public ExpenseFormStatus NewStatus { get; set; }
        public string ActionBy { get; set; } 
        public string Remarks { get; set; } 
    }
}