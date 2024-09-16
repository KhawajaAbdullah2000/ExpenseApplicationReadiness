using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExpenseApplication.Models.ViewModels
{
    public class ExpenseFormViewModel
    {

        public int Id { get; set; }
        public int UserId { get; set; }

        [Required(ErrorMessage = "Currency is required")]
        public string Currency { get; set; }
        public IEnumerable<SelectListItem> CurrencyList { get; set; }
        public List<ExpenseViewModel> Expenses { get; set; }

        public decimal TotalAmount
        {
            get
            {
                return Expenses != null ? Expenses.Sum(e => e.Amount) : 0;
            }
            set { }
        }

        public DateTime DateSubmitted { get; set; }
    }
}