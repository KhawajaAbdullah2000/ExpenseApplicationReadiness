using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExpenseApplication.Models.ViewModels
{
    public class ExpenseFormDetailsViewModel
    {
        public ExpenseForm ExpenseForm { get; set; }
        public string LatestRemark { get; set; }
    }
}