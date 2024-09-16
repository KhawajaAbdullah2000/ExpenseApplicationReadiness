using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExpenseApplication.Models.ViewModels
{
    public class ChangeStatusViewModel
    {
        public ExpenseForm ExpenseForm { get; set; }
        public string Remarks { get; set; }
    }
}